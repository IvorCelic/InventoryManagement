using InventoryManagementAPP.Data;
using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionItemController : InventoryManagementController<InventoryTransactionItem, InventoryTransactionItemDTORead, InventoryTransactionItemDTOInsertUpdate>
    {
        public InventoryTransactionItemController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.InventoryTransactionItems;
            _mapper = new InventoryTransactionItemMapper();
        }


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


        protected override void ControlDelete(InventoryTransactionItem entity)
        {
        }


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


    }
}
