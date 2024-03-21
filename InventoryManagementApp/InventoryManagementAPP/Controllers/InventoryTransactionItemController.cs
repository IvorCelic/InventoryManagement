using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for InventoryTransactions entity CRUD operations.
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
                    //.Include(iti => iti.InventoryTransaction)
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


    }
}
