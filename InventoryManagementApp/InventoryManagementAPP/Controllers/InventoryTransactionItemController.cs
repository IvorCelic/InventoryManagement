﻿using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for InventoryTransactionItems entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionItemController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public InventoryTransactionItemController(InventoryManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all inventory transaction items from the database.
        /// </summary>
        /// <returns>
        /// <response code="200">Success - Returns the list of inventory transactions.</response>
        /// <response code="204">No Content - If no inventory transactions found.</response>
        /// <response code="400">Bad Request - If the request is invalid.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var list = _context.InventoryTransactionItems
                    .Include(iti => iti.InventoryTransaction) // InventoryTransactionItem -> ITI
                        .ThenInclude(ts => ts.TransactionStatus) // TransactionStatus -> TS
                    .Include(iti => iti.Product)
                    .Include(iti => iti.Warehouse)
                    .ToList();

                if (list == null || list.Count == 0)
                {
                    return new EmptyResult();
                }

                return new JsonResult(list.MapInventoryTransactionItemReadList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(InventoryTransactionItemDTOInsertUpdate inventoryTransactionItemDTO)
        {
            if (!ModelState.IsValid || inventoryTransactionItemDTO == null)
            {
                return BadRequest();
            }

            var inventoryTransaction = _context.InventoryTransactions
                .Include(it=>it.TransactionStatus).FirstOrDefault(it=>it.Id== inventoryTransactionItemDTO.inventoryTransactionId);

            if (inventoryTransaction == null)
            {
                return BadRequest();
            }

            var warehouse = _context.Warehouses.Find(inventoryTransactionItemDTO.warehouseId);

            if (warehouse == null)
            {
                return BadRequest();
            }

            var product = _context.Products.Find(inventoryTransactionItemDTO.productId);

            if (product == null)
            {
                return BadRequest();
            }

            var entity = inventoryTransactionItemDTO.MapInventoryTransactionItemInsertUpdateFromDTO(new InventoryTransactionItem());

            entity.InventoryTransaction = inventoryTransaction;
            entity.Warehouse = warehouse;
            entity.Product = product;
            

            try
            {
                _context.InventoryTransactionItems.Add(entity);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, entity.MapInventoryTransactionItemReadToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Put(int id, InventoryTransactionItemDTOInsertUpdate inventoryTransactionItemDTO)
        {
            if (id <= 0 || !ModelState.IsValid || inventoryTransactionItemDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var entity = _context.InventoryTransactionItems
                    .Include(transaction => transaction.InventoryTransaction)
                    .Include(transaction => transaction.Warehouse)
                    .Include(transaction => transaction.Product)
                    .FirstOrDefault(x => x.Id == id);

                if (entity == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                var inventoryTransaction = _context.InventoryTransactions.Find(inventoryTransactionItemDTO.inventoryTransactionId);
                if (inventoryTransaction == null)
                {
                    return BadRequest();
                }

                var warehouse = _context.Warehouses.Find(inventoryTransactionItemDTO.warehouseId);
                if (warehouse == null)
                {
                    return BadRequest();
                }

                var product = _context.Products.Find(inventoryTransactionItemDTO.productId);
                if (product == null)
                {
                    return BadRequest();
                }

                entity = inventoryTransactionItemDTO.MapInventoryTransactionItemInsertUpdateFromDTO(entity);
                entity.Warehouse = warehouse;
                entity.Product = product;

                _context.InventoryTransactionItems.Update(entity);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                return BadRequest();
            }

            try
            {
                var inventoryTransactionItemFromDB = _context.InventoryTransactionItems.Find(id);

                if (inventoryTransactionItemFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                _context.InventoryTransactionItems.Remove(inventoryTransactionItemFromDB);
                _context.SaveChanges();

                return new JsonResult(new { message = "Item from transaction deleted successfully." });
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
                BadRequest();
            }
            try
            {
                var transactionItems = _context.InventoryTransactionItems.Include(it => it.Product).Where(x => x.InventoryTransaction.Id == transactionId).ToList();

                if (transactionItems == null)
                {
                    return BadRequest();
                }

                var products = new List<Product>();
                foreach (var i in transactionItems)
                {
                    products.Add(i.Product);
                }


                return new JsonResult(products.MapProductReadList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
        /*

        [HttpGet]
        [Route("Warehouses/{transactionId:int}")]
        public IActionResult GetWarehouses(int transactionId)
        {
            if (!ModelState.IsValid || transactionId <= 0)
            {
                BadRequest();
            }

            try
            {
                var transaction = _context.InventoryTransactions.Include(it => it.Warehouses).FirstOrDefault(x => x.Id == transactionId);

                if (transaction == null)
                {
                    return BadRequest();
                }

                return new JsonResult(transaction.Warehouses!.MapWarehouseReadList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
        */

        [HttpGet]
        [Route("Transactions/{transactionId:int}/Warehouses/{warehouseId:int}")]
        public IActionResult GetProductsOnWarehouse(int transactionId, int warehouseId)
        {
            if (!ModelState.IsValid || transactionId <= 0 | warehouseId <= 0)
            {
                return BadRequest();
            }

            try
            {
                var productsInWarehouse = _context.InventoryTransactionItems
               //     .Include(i=> i.Warehouse)
                    .Include(i=>i.Product)
                    .Where(iti => iti.InventoryTransaction.Id == transactionId && iti.Warehouse.Id == warehouseId)
                    .ToList();

                if (productsInWarehouse == null)
                {
                    return BadRequest();
                }

                var products = new List<Product>();
                foreach(var i in productsInWarehouse)
                {
                    products.Add(i.Product);
                }


                return new JsonResult(products.MapProductReadList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


    }
}
