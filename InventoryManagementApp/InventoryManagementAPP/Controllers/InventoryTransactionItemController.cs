using InventoryManagementAPP.Data;
using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Controller for managing InventoryTransactionItems-related operations within the inventory management system.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionItemController : InventoryManagementController<InventoryTransactionItem, InventoryTransactionItemDTORead, InventoryTransactionItemDTOInsertUpdate>
    {
        /// <summary>
        /// Constructor for InventoryTransactionItemController.
        /// Initializes the controller with the specified context, sets the DbSet to InventoryTransactionItems, 
        /// and initializes the mapper for this controller.
        /// </summary>
        /// <param name="context">The InventoryManagementContext for the controller.</param>
        public InventoryTransactionItemController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.InventoryTransactionItems;
            _mapper = new InventoryTransactionItemMapper();
        }

        /// <summary>
        /// Edits an existing Inventory Transaction Item entity based on the given DTO. This method assigns 
        /// the related inventory transaction, warehouse, and product to the entity based on the provided DTO.
        /// </summary>
        /// <param name="entityDTO">The DTO containing the updated details of the transaction item.</param>
        /// <param name="entity">The existing InventoryTransactionItem entity to be updated.</param>
        /// <returns>Returns the updated InventoryTransactionItem entity.</returns>
        protected override InventoryTransactionItem EditEntity(InventoryTransactionItemDTOInsertUpdate entityDTO, InventoryTransactionItem entity)
        {
            var transaction = _context.InventoryTransactions.Find(entityDTO.transactionId) ?? throw new Exception("There is no Inventory Transaction with ID: " + entityDTO.transactionId + " in database.");
            var warehouse = _context.Warehouses.Find(entityDTO.warehouseId) ?? throw new Exception("There is no Warehouse with ID: " + entityDTO.warehouseId + " in database.");
            var product = _context.Products.Find(entityDTO.productId) ?? throw new Exception("There is no Product with ID: " + entityDTO.productId + " in database.");

            entity.InventoryTransaction = transaction;
            entity.Warehouse = warehouse;
            entity.Product = product;
            entity.Quantity = entityDTO.quantity;

            return entity;
        }

        /// <summary>
        /// Loads an InventoryTransactionItem entity based on its ID, including its associated InventoryTransaction,
        /// Warehouse, and Product information. Throws an exception if the item does not exist.
        /// </summary>
        /// <param name="id">The ID of the InventoryTransactionItem to be loaded.</param>
        /// <returns>Returns the loaded InventoryTransactionItem entity.</returns>
        protected override InventoryTransactionItem LoadEntity(int id)
        {
            var entity = _context.InventoryTransactionItems
                .Include(it => it.InventoryTransaction)
                .Include(it => it.Warehouse)
                .Include(it => it.Product)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new Exception("There is no Transaction Item with ID: " + id + " in database.");
            }

            return entity;
        }

        /// <summary>
        /// Loads all InventoryTransactionItem entities from the context, including related InventoryTransaction,
        /// Warehouse, and Product information. Throws an exception if there is no data.
        /// </summary>
        /// <returns>Returns a list of InventoryTransactionItemDTORead mapped from the entities.</returns>
        protected override List<InventoryTransactionItemDTORead> LoadEntites()
        {
            var list = _context.InventoryTransactionItems
                .Include(it => it.InventoryTransaction)
                .Include(it => it.Warehouse)
                .Include(it => it.Product)
                .ToList();

            if (list == null || list.Count == 0)
            {
                throw new Exception("No data in database.");
            }

            return _mapper.MapReadList(list);
        }


        protected override InventoryTransactionItem CreateEntity(InventoryTransactionItemDTOInsertUpdate entityDTO)
        {
            var transaction = _context.InventoryTransactions.Find(entityDTO.transactionId);
            if (transaction == null)
            {
                throw new Exception("There is no Inventory Transaction with ID: " + transaction.Id + " in database.");
            }

            var warehouse = _context.Warehouses.Find(entityDTO.warehouseId);
            if (warehouse == null)
            {
                throw new Exception("There is no Warehouse with ID: " + warehouse.Id + " in database.");
            }

            var product = _context.Products.Find(entityDTO.productId);
            if (product == null)
            {
                throw new Exception("There is no Product with ID: " + product.Id + " in database.");
            }

            var existingItem = _context.InventoryTransactionItems
                .FirstOrDefault(item =>
                item.InventoryTransaction.Id == entityDTO.transactionId &&
                item.Product.Id == entityDTO.productId);

            if (existingItem != null)
            {
                throw new Exception("The product already exists in this transaction.");
            }

            var entity = _mapper.MapInsertUpdatedFromDTO(entityDTO);
            entity.InventoryTransaction = transaction;
            entity.Product = product;
            entity.Warehouse = warehouse;

            return entity;
        }

        /// <summary>
        /// Custom logic to control the deletion of an InventoryTransactionItem entity.
        /// </summary>
        /// <param name="entity">The InventoryTransactionItem entity to be deleted.</param>
        protected override void ControlDelete(InventoryTransactionItem entity)
        {
        }

        /// <summary>
        /// Retrieves a list of distinct warehouses associated with a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <returns>Returns a JSON result with a list of WarehouseDTORead for the given transaction ID.</returns>
        [HttpGet]
        [Route("Warehouses/{transactionId:int}")]
        public IActionResult GetWarehouses(int transactionId)
        {
            if (!ModelState.IsValid || transactionId <= 0)
            {
                BadRequest(ModelState);
            }

            try
            {
                var warehouses = _context.InventoryTransactionItems
                    .Include(it => it.Warehouse)
                    .Where(x => x.InventoryTransaction.Id == transactionId)
                    .Select(x => x.Warehouse)
                    .Distinct()
                    .ToList();

                if (warehouses == null)
                {
                    return BadRequest();
                }

                var mapping = new Mapping<Warehouse, WarehouseDTORead, WarehouseDTOInsertUpdate>();

                return new JsonResult(mapping.MapReadList(warehouses));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Searches for unassociated products with a given transaction ID and condition, 
        /// filtering the results based on a search condition.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <param name="condition">A search condition to filter the unassociated products.</param>
        /// <returns>Returns a JSON result with a list of unassociated products matching the condition.</returns>
        [HttpGet]
        [Route("SearchUnassociatedProduct/{transactionId:int}/{condition}")]
        public IActionResult SearchUnassociatedProduct(int transactionId, string condition)
        {
            if (condition == null || condition.Length < 3 || transactionId <= 0)
            {
                return BadRequest(ModelState);
            }

            condition = condition.ToLower();
            try
            {
                var allProducts = _context.Products.ToList();

                var associatedProducts = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Include(i => i.InventoryTransaction)
                    .Where(x => x.InventoryTransaction.Id == transactionId)
                    .ToList();

                var unassociatedProducts = allProducts
                    .Where(p => !associatedProducts.Any(ap => ap.Product.Id == p.Id))
                    .ToList();

                var list = new List<ProductDTORead>();
                unassociatedProducts.ForEach(product =>
                {
                    list.Add(new ProductDTORead(
                        product.Id,
                        product.ProductName,
                        product.Description,
                        product.IsUnitary
                    ));
                });

                var filteredProducts = new List<ProductDTORead>();
                foreach (var product in list)
                {
                    if (product.productName.ToLower().Contains(condition))
                    {
                        filteredProducts.Add(product);
                    }
                }

                return new JsonResult(filteredProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of unassociated products for a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <returns>Returns a JSON result with a list of paginated unassociated products.</returns>
        [HttpGet]
        [Route("SearchProductOnWarehouse/{transactionId:int}/{warehouseId:int}/{condition}")]
        public IActionResult SearchProductOnWarehouse(int transactionId, int warehouseId, string condition)
        {
            if (condition == null || condition.Length < 3 || transactionId <= 0 || warehouseId <= 0)
            {
                return BadRequest(ModelState);
            }

            condition = condition.ToLower();
            try
            {
                var productsInWarehouse = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Include(i => i.Warehouse)
                    .Include(i => i.InventoryTransaction)
                    .Where(iti => iti.InventoryTransaction.Id == transactionId && iti.Warehouse.Id == warehouseId)
                    .ToList();

                if (productsInWarehouse == null)
                {
                    return BadRequest();
                }

                var list = new List<ProductsOnTransactionDTORead>();
                productsInWarehouse.ForEach(product =>
                {
                    list.Add(new ProductsOnTransactionDTORead(
                        product.Id,
                        product.Product.ProductName,
                        product.Product.IsUnitary,
                        product.Quantity
                        ));
                });

                var filteredProducts = new List<ProductsOnTransactionDTORead>();
                foreach (var product in list)
                {
                    if (product.productName.ToLower().Contains(condition))
                    {
                        filteredProducts.Add(product);
                    }
                }

                return new JsonResult(filteredProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of unassociated products for a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <returns>Returns a JSON result with a list of paginated unassociated products.</returns>
        [HttpGet]
        [Route("UnassociatedProductsPagination/{transactionId:int}/{page}")]
        public IActionResult UnassociatedProductsPagination(int transactionId, int page)
        {
            var perPage = 8;

            try
            {
                var allProducts = _context.Products.ToList();

                var associatedProducts = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Include(i => i.InventoryTransaction)
                    .Where(x => x.InventoryTransaction.Id == transactionId)
                    .ToList();

                var unassociatedProducts = allProducts
                    .Where(p => !associatedProducts.Any(ap => ap.Product.Id == p.Id))
                    .Skip((perPage * page) - perPage)
                    .Take(perPage)
                    .ToList();

                var list = new List<ProductDTORead>();
                unassociatedProducts.ForEach(product =>
                {
                    list.Add(new ProductDTORead(
                        product.Id,
                        product.ProductName,
                        product.Description,
                        product.IsUnitary
                    ));
                });

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Searches for products within a specific warehouse for a given transaction ID and warehouse ID, 
        /// filtering the results based on a search condition.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <param name="warehouseId">The ID of the warehouse to query.</param>
        /// <param name="condition">A search condition to filter the products within the warehouse.</param>
        /// <returns>Returns a JSON result with a list of filtered products within the warehouse.</returns>
        [HttpGet]
        [Route("ProductsOnWarehousePagination/{transactionId:int}/{warehouseId:int}/{page}")]
        public IActionResult ProductsOnWarehousePagination(int transactionId, int warehouseId, int page)
        {
            var perPage = 8;

            //if (transactionId <= 0 || warehouseId <= 0 || page <= 0)
            //{
            //    return BadRequest(ModelState);
            //}
            try
            {
                var productsInWarehouse = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Include(i => i.Warehouse)
                    .Include(i => i.InventoryTransaction)
                    .Where(iti => iti.InventoryTransaction.Id == transactionId && iti.Warehouse.Id == warehouseId)
                    .Skip((perPage * page) - perPage)
                    .Take(perPage)
                    .ToList();

                if (productsInWarehouse == null)
                {
                    return BadRequest();
                }

                var list = new List<ProductsOnTransactionDTORead>();
                productsInWarehouse.ForEach(product =>
                {
                    list.Add(new ProductsOnTransactionDTORead(
                        product.Id,
                        product.Product.ProductName,
                        product.Product.IsUnitary,
                        product.Quantity
                        ));
                });

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of products within a specific warehouse for a given transaction ID and warehouse ID.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <param name="warehouseId">The ID of the warehouse to query.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <returns>Returns a JSON result with a list of paginated products within the warehouse.</returns>
        [HttpGet]
        [Route("UnassociatedProducts/{transactionId:int}")]
        public IActionResult GetUnassociatedProducts(int transactionId)
        {
            if (!ModelState.IsValid || transactionId <= 0)
            {
                BadRequest(ModelState);
            }
            try
            {
                var allProducts = _context.Products.ToList();

                if (allProducts == null)
                {
                    return BadRequest();
                }

                var associatedProducts = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Include(i => i.InventoryTransaction)
                    .Where(x => x.InventoryTransaction.Id == transactionId)
                    .ToList();

                var unassociatedProducts = allProducts
                    .Where(p => !associatedProducts.Any(ap => ap.Product.Id == p.Id))
                    .ToList();

                var list = new List<ProductDTORead>();
                unassociatedProducts.ForEach(product =>
                {
                    list.Add(new ProductDTORead(
                        product.Id,
                        product.ProductName,
                        product.Description,
                        product.IsUnitary
                        )); ;
                });

                return new JsonResult(list);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        /// <summary>
        /// Retrieves all products associated with a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <returns>Returns a JSON result with a list of products associated with the specified transaction ID.</summary>
        [HttpGet]
        [Route("Products/{transactionId:int}")]
        public IActionResult GetProducts(int transactionId)
        {
            if (!ModelState.IsValid || transactionId <= 0)
            {
                BadRequest(ModelState);
            }
            try
            {
                var products = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Include(i => i.InventoryTransaction)
                    .Where(x => x.InventoryTransaction.Id == transactionId)
                    .ToList();

                if (products == null)
                {
                    return BadRequest();
                }

                var list = new List<ProductsOnTransactionDTORead>();
                products.ForEach(product =>
                {
                    list.Add(new ProductsOnTransactionDTORead(
                        product.Product.Id,
                        product.Product.ProductName,
                        product.Product.IsUnitary,
                        product.Quantity
                        ));
                });

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves products within a specific warehouse for a given transaction ID and warehouse ID.
        /// </summary>
        /// <param name="transactionId">The ID of the inventory transaction to query.</param>
        /// <param name="warehouseId">The ID of the warehouse to query.</param>
        /// <returns>Returns a JSON result with a list of products within the specified warehouse for a given transaction ID.</summary>
        [HttpGet]
        [Route("Transactions/{transactionId:int}/Warehouses/{warehouseId:int}")]
        public IActionResult GetProductsOnWarehouse(int transactionId, int warehouseId)
        {
            if (!ModelState.IsValid || transactionId <= 0 || warehouseId <= 0)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var productsInWarehouse = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Include(i => i.Warehouse)
                    .Include(i => i.InventoryTransaction)
                    .Where(iti => iti.InventoryTransaction.Id == transactionId && iti.Warehouse.Id == warehouseId)
                    .ToList();

                if (productsInWarehouse == null)
                {
                    return BadRequest();
                }

                var list = new List<ProductsOnTransactionDTORead>();
                productsInWarehouse.ForEach(product =>
                {
                    list.Add(new ProductsOnTransactionDTORead(
                        product.Id,
                        product.Product.ProductName,
                        product.Product.IsUnitary,
                        product.Quantity
                        ));
                });

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        [HttpGet]
        [Route("InventoryTransactionReport")]
        public IActionResult GenerateInventoryTransactionReport(int transactionId)
        {
            if (transactionId <= 0)
            {
                return BadRequest("Invalid transaction ID.");
            }

            try
            {
                using (var pdfStream = new MemoryStream())
                {
                    Document document = new Document();
                    // Explicitly refer to iTextSharp.text.pdf.PdfWriter
                    var pdfWriter = PdfWriter.GetInstance(document, pdfStream);

                    document.Open();

                    var content = GenerateInventoryTransactionContent(transactionId);
                    foreach (var element in content)
                    {
                        document.Add(element);
                    }

                    document.Close();

                    return new FileContentResult(pdfStream.ToArray(), "application/pdf")
                    {
                        FileDownloadName = $"InventoryTransaction_{transactionId}.pdf"
                    };
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating PDF report: {ex.Message}");
            }
        }


        private List<IElement> GenerateInventoryTransactionContent(int transactionId)
        {
            var transaction = GetInventoryTransaction(transactionId);

            if (transaction == null)
            {
                throw new Exception($"Inventory Transaction with ID {transactionId} not found.");
            }

            var transactionItems = GetInventoryTransactionItems(transactionId);

            if (transactionItems == null)
            {
                throw new Exception($"Inventory Transaction with ID {transactionId} not found.");
            }

            var elements = new List<IElement>();

            var header = new Paragraph($"Inventory Transaction Report for: {transaction.AdditionalDetails}", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
            elements.Add(header);

            foreach (var item in transactionItems)
            {
                var productDetail = new Paragraph(
                    $"Product: {item.Product.ProductName}, Warehouse: {item.Warehouse.WarehouseName}",
                    new Font(Font.FontFamily.HELVETICA, 12)
                    );
                elements.Add(productDetail);
            }

            var htmlContent = "<p>This is an <strong>inventory transaction report</strong>.</p>";
            using (var stringReader = new StringReader(htmlContent))
            {
                var htmlElements = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(stringReader, null);
                elements.AddRange(htmlElements);
            }

            return elements;
        }


        private List<InventoryTransactionItem> GetInventoryTransactionItems(int transactionId)
        {
            var transactionItems = _context.InventoryTransactionItems
                .Include(it => it.InventoryTransaction)
                .Include(w => w.Warehouse)
                .Include(p => p.Product)
                .Where(it => it.InventoryTransaction.Id == transactionId)
                .ToList();

            return transactionItems;
        }


        private InventoryTransaction GetInventoryTransaction(int transactionId)
        {
            var transaction = _context.InventoryTransactions
                .Include(e => e.Employee)
                .FirstOrDefault(t => t.Id == transactionId);

            return transaction;
        }



    }
}
