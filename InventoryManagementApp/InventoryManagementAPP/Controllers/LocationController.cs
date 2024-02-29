using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Intended for CRUD operations  on entity location in database
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationController : ControllerBase
    {
        /// <summary>
        /// Context for work with database which will be setup with Dependency Injection
        /// </summary>
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Constructor of class which takes Inventory Management context with DI principal
        /// </summary>
        /// <param name="context"></param>
        public LocationController(InventoryManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all locations from database
        /// </summary>
        /// <remarks>
        ///     Example:
        ///     GET api/v1/Location
        /// </remarks>
        /// <returns>Locations in database</returns>
        /// <response code="200">Everything OK, if no data then content-length: 0</response>
        /// <response code="400">Bad request</response>
        /// <response code="503">Database on which I am connecting is not available</response>
        [HttpGet]
        public IActionResult Get()
        {
            // control if request is not valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var locations = _context.Locations.ToList();

                if (locations == null || locations.Count == 0)
                {
                    return new EmptyResult();
                }

                return new JsonResult(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
    }
}
