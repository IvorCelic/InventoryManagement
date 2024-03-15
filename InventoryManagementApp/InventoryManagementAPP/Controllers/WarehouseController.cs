using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for Warehouses entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseController"/> class.
        /// </summary>
        /// <param name="context">The Inventory Management context for database interaction.</param>
        public WarehouseController(InventoryManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all warehouses from the database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/Warehouse
        /// </remarks>
        /// <returns>Returns a list of warehouses in the database.</returns>
        /// <response code="200">Success - Returns the list of warehouses.</response>
        /// <response code="400">Bad request - If the request is invalid.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpGet]
        public IActionResult Get()
        {
            // Validate the model state.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Retrieve all locations from the database.
                var warehouses = _context.Warehouses.ToList();

                // Check if locations are found.
                if (warehouses == null || warehouses.Count == 0)
                {
                    // Return a JSON result with a message when no locations are found.
                    return new EmptyResult();
                }

                // Return a JSON result with the list of locations.
                return new JsonResult(warehouses.MapWarehouseReadList());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a warehouse from the database based on the specified ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/Warehouse/{id}
        /// </remarks>
        /// <param name="id">The ID of the warehouse to retrieve.</param>
        /// <returns>Returns the requested warehouse if found.</returns>
        /// <response code="200">OK - Returns the requested warehouse.</response>
        /// <response code="204">No Content - If the specified warehouse with the given ID is not found.</response>
        /// <response code="400">Bad Request - If the request is invalid or ID is less than or equal to 0.</response>
        /// <response code="503">Service Unavailable - If there is an issue accessing the database.</response>
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            // Validate the model state and ID.
            if (!ModelState.IsValid || id <= 0)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Retrieve the warehouse from the database based on the provided ID.
                var warehouse = _context.Warehouses.Find(id);

                // Check if the warehouse is not found.
                if (warehouse == null)
                {
                    // Return an empty result when the warehouse is not found.
                    return new EmptyResult();
                }

                // Return the found warehouse as a JSON result.
                return new JsonResult(warehouse.MapWarehouseInsertUpdatedToDTO());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new warehouse.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST api/v1/Warehouse
        ///     { "name": "example of name", "description": "example description" }
        /// </remarks>
        /// <param name="warehouseDTO">The warehouse to insert in JSON format.</param>
        /// <returns>Returns the newly created warehouse with its ID.</returns>
        /// <response code="201">Created - Returns the newly created warehouse.</response>
        /// <response code="400">Bad request - If the request is invalid or warehouse is null.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(WarehouseDTOInsertUpdate warehouseDTO)
        {
            // Validate the model state and warehouse.
            if (!ModelState.IsValid || warehouseDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var warehouse = warehouseDTO.MapWarehouseInsertUpdateFromDTO(new Warehouse());
                // Add the new warehouse to the database and save changes.
                _context.Warehouses.Add(warehouse);
                _context.SaveChanges();

                // Return a JSON result with the newly created warehouse.
                return StatusCode(StatusCodes.Status201Created, warehouse.MapWarehouseReadToDTO());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Updates the data of an existing warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse to update.</param>
        /// <param name="warehouseDTO">The updated warehouse data in JSON format.</param>
        /// <returns>Returns the updated warehouse.</returns>
        /// <response code="200">OK - Returns the updated warehouse.</response>
        /// <response code="400">Bad request - If the request is invalid, warehouse is null, or ID is less than or equal to 0.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Put(int id, WarehouseDTOInsertUpdate warehouseDTO)
        {
            // Validate the model state, ID, and warehouse.
            if (id <= 0 || !ModelState.IsValid || warehouseDTO == null)
            {
                return BadRequest();
            }

            try
            {
                // Find the existing warehouse in the database based on the provided ID.
                var warehouseFromDB = _context.Warehouses.Find(id);

                // Check if the warehouse is not found.
                if (warehouseFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                var warehouse = warehouseDTO.MapWarehouseInsertUpdateFromDTO(warehouseFromDB);

                _context.Warehouses.Update(warehouseFromDB);
                _context.SaveChanges();

                // Return a JSON result with the updated warehouse.
                return StatusCode(StatusCodes.Status200OK, warehouse.MapWarehouseReadToDTO());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse to delete.</param>
        /// <returns>Returns a message indicating successful deletion.</returns>
        /// <response code="200">OK - The warehouse was successfully deleted.</response>
        /// <response code="204">No Content - The specified warehouse was not found.</response>
        /// <response code="400">Bad request - If the request is invalid or ID is less than or equal to 0.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        public IActionResult Delete(int id)
        {
            // Validate the model state and ID.
            if (id <= 0 || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                // Find the warehouse in the database based on the provided ID.
                var warehouseFromDB = _context.Warehouses.Find(id);

                // Check if the warehouse is not found.
                if (warehouseFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Warehouse not found.");
                }

                // Remove the warehouse from the database and save changes.
                _context.Warehouses.Remove(warehouseFromDB);
                _context.SaveChanges();

                // Return a JSON result indicating successful deletion.
                return new JsonResult(new { message = "Warehouse deleted successfully." });
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
    }
}
