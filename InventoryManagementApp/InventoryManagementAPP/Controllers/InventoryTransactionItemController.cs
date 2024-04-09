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
        [Route("Products/{transactionId:int}")]
        public IActionResult GetProducts(int transactionId)
        {
            if (!ModelState.IsValid || transactionId <= 0)
            {
                BadRequest();
            }
            try
            {
                var products = _context.InventoryTransactionItems
                    .Include(it => it.Product)
                    .Where(x => x.InventoryTransaction.Id == transactionId)
                    .ToList();

                if (products == null)
                {
                    return BadRequest();
                }

                //var mapping = new InventoryTransactionMapper();

                return new JsonResult(_mapper.MapReadList(products));
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
                return BadRequest();
            }

            try
            {
                var productsInWarehouse = _context.InventoryTransactionItems
                    .Include(i => i.Product)
                    .Where(iti => iti.InventoryTransaction.Id == transactionId && iti.Warehouse.Id == warehouseId)
                    .ToList();

                if (productsInWarehouse == null)
                {
                    return BadRequest();
                }

                var mapping = new InventoryTransactionMapper();

                return new JsonResult(_mapper.MapReadList(productsInWarehouse));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        [HttpPost]
        [Route("Transactions/{transactionId:int}/Warehouses/{warehouseId:int}/Products/{productId:int}")]
        public IActionResult AddProductOnWarehouse(int transactionId, int warehouseId, int productId, int quantity)
        {
            if (!ModelState.IsValid || transactionId <= 0 || warehouseId <= 0 || productId <= 0)
            {
                return BadRequest();
            }

            try
            {
                var inventoryTransaction = _context.InventoryTransactions.Find(transactionId);
                if (inventoryTransaction == null)
                {
                    return BadRequest();
                }

                var warehouse = _context.Warehouses.Find(warehouseId);
                if (warehouse == null)
                {
                    return BadRequest();
                }

                var product = _context.Products.Find(productId);
                if (product == null)
                {
                    return BadRequest();
                }

                var inventoryTransactionItem = new InventoryTransactionItem
                {
                    InventoryTransaction = inventoryTransaction,
                    Warehouse = warehouse,
                    Product = product,
                    Quantity = quantity
                };

                _context.InventoryTransactionItems.Add(inventoryTransactionItem);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


    }
}
