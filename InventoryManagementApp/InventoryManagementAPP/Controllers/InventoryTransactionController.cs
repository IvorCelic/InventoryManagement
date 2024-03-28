using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for InventoryTransactions entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public InventoryTransactionController(InventoryManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all inventory transactions from the database.
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
                var list = _context.InventoryTransactions
                    .Include(it => it.Employee)
                    .Include(it => it.TransactionStatus)
                    .ToList();

                if (list == null || list.Count == 0)
                {
                    return new EmptyResult();
                }

                return new JsonResult(list.MapInventoryTransactionReadList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a inventory transaction from the database based on the specified ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/InventoryTransaction/{id}
        /// </remarks>
        /// <param name="id">The ID of the inventory transaction to retrieve.</param>
        /// <returns>Returns the requested inventory transaction if found.</returns>
        /// <response code="200">OK - Returns the requested inventory transaction.</response>
        /// <response code="204">No Content - If the specified inventory transaction with the given ID is not found.</response>
        /// <response code="400">Bad Request - If the request is invalid or ID is less than or equal to 0.</response>
        /// <response code="503">Service Unavailable - If there is an issue accessing the database.</response>
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var it = _context.InventoryTransactions
                    .Include(transaction => transaction.Employee)
                    .Include(transaction => transaction.TransactionStatus)
                    .FirstOrDefault(x => x.Id == id);

                if (it == null)
                {
                    return new EmptyResult();
                }

                return new JsonResult(it.MapInventoryTransactionInsertToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new inventory transaction.
        /// </summary>
        /// <param name="inventoryTransactionDTO">Inventory transaction to be created.</param>
        /// <returns>
        /// <response code="201">Created - Returns the created inventory transaction.</response>
        /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(InventoryTransactionDTOInsert inventoryTransactionDTO)
        {
            if (!ModelState.IsValid || inventoryTransactionDTO == null)
            {
                return BadRequest();
            }

            var employee = _context.Employees.Find(inventoryTransactionDTO.employeeId);

            if (employee == null)
            {
                return BadRequest();
            }

            var transactionStatus = _context.TransactionStatuses.Find(inventoryTransactionDTO.transactionStatusId);

            if (transactionStatus == null)
            {
                return BadRequest();
            }

            var entity = inventoryTransactionDTO.MapInventoryTransactionInsertFromDTO(new InventoryTransaction());

            entity.Employee = employee;
            entity.TransactionStatus = transactionStatus;

            try
            {
                _context.InventoryTransactions.Add(entity);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, entity.MapInventoryTransactionReadToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Updates the data of an existing inventory transaction.
        /// </summary>
        /// <param name="id">The ID of inventory transaction to be updated.</param>
        /// <param name="inventoryTransactionDTO">The updated inventory transaction information.</param>
        /// <returns>
        /// <response code="200">OK - Returns the updated inventory transaction.</response>
        /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
        /// <response code="204">No Content - If the specified inventory transaction with the given ID is not found.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Put(int id, InventoryTransactionDTOUpdate inventoryTransactionDTO)
        {
            if (id <= 0 || !ModelState.IsValid || inventoryTransactionDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var entity = _context.InventoryTransactions
                    .Include(transaction => transaction.Employee)
                    .Include(transaction => transaction.TransactionStatus)
                    .FirstOrDefault(x => x.Id == id);

                if (entity == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                var transactionStatus = _context.TransactionStatuses.Find(inventoryTransactionDTO.transactionStatusId);

                if (transactionStatus == null)
                {
                    return BadRequest();
                }

                var employee = _context.Employees.Find(inventoryTransactionDTO.employeeId);

                if (employee == null)
                {
                    return BadRequest();
                }

                entity = inventoryTransactionDTO.MapInventoryTransactionUpdateFromDTO(entity);

                entity.TransactionStatus = transactionStatus;
                entity.Employee = employee;

                _context.InventoryTransactions.Update(entity);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        /// <summary>
        /// Deletes the specified inventory transaction.
        /// </summary>
        /// <param name="id">The ID of inventory transaction to be deleted.</param>
        /// <returns>
        /// <response code="200">OK - Returns a success message.</response>
        /// <response code="204">No Content - If no inventory transaction found with the specified ID.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
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
                var inventoryTransactionFromDB = _context.InventoryTransactions.Find(id);

                if (inventoryTransactionFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                _context.InventoryTransactions.Remove(inventoryTransactionFromDB);
                _context.SaveChanges();

                return new JsonResult(new { message = "Inventory transaction deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


    }
}
