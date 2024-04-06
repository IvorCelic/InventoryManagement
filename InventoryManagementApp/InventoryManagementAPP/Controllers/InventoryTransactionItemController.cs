using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for InventoryTransactionItems entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionItemController : InventoryManagementController<InventoryTransactionItem, InventoryTransactionItemDTORead, InventoryTransactionItemDTOInsertUpdate>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public InventoryTransactionItemController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.InventoryTransactionItems;
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
                var inventoryTransactionItems = _context.InventoryTransactionItems
                    .Include(it => it.Product)
                    .Where(x => x.InventoryTransaction.Id == transactionId)
                    .ToList();

                if (inventoryTransactionItems == null)
                {
                    return BadRequest();
                }

                var productsOnTransaction = inventoryTransactionItems.MapToProductWithQuantityDTOList();

                return new JsonResult(productsOnTransaction);
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

                var productsWithQuantities = productsInWarehouse.MapToProductWithQuantityDTOList();

                return new JsonResult(productsWithQuantities);
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


        protected override void ControlDelete(InventoryTransactionItem entity)
        {
        }


    }
}
