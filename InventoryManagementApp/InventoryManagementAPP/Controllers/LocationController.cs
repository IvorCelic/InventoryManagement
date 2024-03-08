using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

/// <summary>
/// Inventory Management API controllers for Locations entity CRUD operations.
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
        // Validate the model state.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Retrieve all locations from the database.
            var locations = _context.Locations.ToList();

            // Check if locations are found.
            if (locations == null || locations.Count == 0)
            {
                // Return a JSON result with a message when no locations are found.
                return new JsonResult("No locations found.") { StatusCode = StatusCodes.Status204NoContent };
            }

            // Return a JSON result with the list of locations.
            return new JsonResult(locations);
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a location from the database based on the specified ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     GET api/v1/Location/{id}
    /// </remarks>
    /// <param name="id">The ID of the location to retrieve.</param>
    /// <returns>Returns the requested location if found.</returns>
    /// <response code="200">OK - Returns the requested location.</response>
    /// <response code="204">No Content - If the specified location with the given ID is not found.</response>
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
            // Retrieve the location from the database based on the provided ID.
            var location = _context.Locations.Find(id);

            // Check if the location is not found.
            if (location == null)
            {
                // Return an empty result when the location is not found.
                return new EmptyResult();
            }

            // Return the found location as a JSON result.
            return new JsonResult(location)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
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
        // Validate the model state and location.
        if (!ModelState.IsValid || location == null)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Add the new location to the database and save changes.
            _context.Locations.Add(location);
            _context.SaveChanges();

            // Return a JSON result with the newly created location.
            return StatusCode(StatusCodes.Status201Created, location);
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
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
        // Validate the model state, ID, and location.
        if (id <= 0 || !ModelState.IsValid || location == null)
        {
            return BadRequest();
        }

        try
        {
            // Find the existing location in the database based on the provided ID.
            var locationFromDB = _context.Locations.Find(id);

            // Check if the location is not found.
            if (locationFromDB == null)
            {
                return BadRequest();
            }

            // Update the location data and save changes.
            locationFromDB.Name = location.Name;
            locationFromDB.Description = location.Description;
            _context.Locations.Update(locationFromDB);
            _context.SaveChanges();

            // Return a JSON result with the updated location.
            return StatusCode(StatusCodes.Status200OK, locationFromDB);
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
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
        // Validate the model state and ID.
        if (id <= 0 || !ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            // Find the location in the database based on the provided ID.
            var locationFromDB = _context.Locations.Find(id);

            // Check if the location is not found.
            if (locationFromDB == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "Location not found.");
            }

            // Remove the location from the database and save changes.
            _context.Locations.Remove(locationFromDB);
            _context.SaveChanges();

            // Return a JSON result indicating successful deletion.
            return new JsonResult("Location deleted successfully.") { StatusCode = StatusCodes.Status200OK };
        }
        catch (SqlException ex)
        {
            // Handle and return a service unavailable status with the SQL error code.
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.ErrorCode);
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
    }
}
