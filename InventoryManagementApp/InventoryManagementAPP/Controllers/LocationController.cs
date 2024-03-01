using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Controller for performing CRUD operations on the Location entity in the database.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationController"/> class.
        /// </summary>
        /// <param name="context">The Inventory Management context for database interaction.</param>
        public LocationController(InventoryManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all locations from the database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/Location
        /// </remarks>
        /// <returns>Returns a list of locations in the database.</returns>
        /// <response code="200">Success - Returns the list of locations.</response>
        /// <response code="400">Bad request - If the request is invalid.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                var locations = _context.Locations.ToList();

                if (locations == null || locations.Count == 0)
                {
                    return new JsonResult("No locations found.") { StatusCode = StatusCodes.Status204NoContent };
                }

                return new JsonResult(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new location.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST api/v1/Location
        ///     { "name": "example of name", "description": "example description" }
        /// </remarks>
        /// <param name="location">The location to insert in JSON format.</param>
        /// <returns>Returns the newly created location with its ID.</returns>
        /// <response code="201">Created - Returns the newly created location.</response>
        /// <response code="400">Bad request - If the request is invalid or location is null.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(Location location)
        {
            if (!ModelState.IsValid || location == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Locations.Add(location);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, location);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Updates the data of an existing location.
        /// </summary>
        /// <param name="id">The ID of the location to update.</param>
        /// <param name="location">The updated location data in JSON format.</param>
        /// <returns>Returns the updated location.</returns>
        /// <response code="200">OK - Returns the updated location.</response>
        /// <response code="400">Bad request - If the request is invalid, location is null, or ID is less than or equal to 0.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Put(int id, Location location)
        {
            if (id <= 0 || !ModelState.IsValid || location == null)
            {
                return BadRequest();
            }

            try
            {
                var locationFromDB = _context.Locations.Find(id);

                if (locationFromDB == null)
                {
                    return BadRequest();
                }

                locationFromDB.Name = location.Name;
                locationFromDB.Description = location.Description;

                _context.Locations.Update(locationFromDB);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, locationFromDB);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified location.
        /// </summary>
        /// <param name="id">The ID of the location to delete.</param>
        /// <returns>Returns a message indicating successful deletion.</returns>
        /// <response code="200">OK - The location was successfully deleted.</response>
        /// <response code="204">No Content - The specified location was not found.</response>
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
            if (id <= 0 || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var locationFromDB = _context.Locations.Find(id);

                if (locationFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Location not found.");
                }

                _context.Locations.Remove(locationFromDB);
                _context.SaveChanges();

                return new JsonResult("Location deleted.") { StatusCode = StatusCodes.Status200OK };
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
    }
}
