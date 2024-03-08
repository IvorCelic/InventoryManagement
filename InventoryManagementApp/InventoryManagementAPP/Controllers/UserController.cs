using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

/// <summary>
/// Inventory Management API controllers for Users entity CRUD operations.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly InventoryManagementContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserController(InventoryManagementContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>
    /// <response code="200">Success - Returns the list of users.</response>
    /// <response code="204">No Content - If no users found.</response>
    /// <response code="400">Bad Request - If the request is invalid.</response>
    /// <response code="503">Service Unavailable - If the database is not accessible.</response>
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get()
    {
        // Validate the model state.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Retrieve all users from the database.
            var users = _context.Users.ToList();

            // Check if users are found.
            if (users == null || users.Count == 0)
            {
                // Return an empty result when no users are found.
                return new EmptyResult();
            }

            // Return a JSON result with the list of users.
            return new JsonResult(users);
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a user from the database based on the specified ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     GET api/v1/User/{id}
    /// </remarks>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>Returns the requested user if found.</returns>
    /// <response code="200">OK - Returns the requested user.</response>
    /// <response code="204">No Content - If the specified user with the given ID is not found.</response>
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
            // Retrieve the user from the database based on the provided ID.
            var user = _context.Users.Find(id);

            // Check if the user is not found.
            if (user == null)
            {
                // Return an empty result when the user is not found.
                return new EmptyResult();
            }

            // Return the found user as a JSON result.
            return new JsonResult(user)
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
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The user to be created.</param>
    /// <returns>
    /// <response code="201">Created - Returns the created user.</response>
    /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
    /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult Post(User user)
    {
        // Validate the model state and user.
        if (!ModelState.IsValid || user == null)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Add the new user to the database and save changes.
            _context.Users.Add(user);
            _context.SaveChanges();

            // Return a JSON result with the newly created user.
            return StatusCode(StatusCodes.Status201Created, user);
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
    }

    /// <summary>
    /// Updates the data of an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to be updated.</param>
    /// <param name="user">The updated user information.</param>
    /// <returns>
    /// <response code="200">OK - Returns the updated user.</response>
    /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
    /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
    /// </returns>
    [HttpPut]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult Put(int id, User user)
    {
        // Validate the model state, ID, and user.
        if (id <= 0 || !ModelState.IsValid || user == null)
        {
            return BadRequest();
        }

        try
        {
            // Find the existing user in the database based on the provided ID.
            var userFromDB = _context.Users.Find(id);

            // Check if the user is not found.
            if (userFromDB == null)
            {
                return BadRequest();
            }

            // Update the user data and save changes.
            userFromDB.FirstName = user.FirstName;
            userFromDB.LastName = user.LastName;
            userFromDB.Email = user.Email;
            userFromDB.Password = user.Password;

            _context.Users.Update(userFromDB);
            _context.SaveChanges();

            // Return a JSON result with the updated user.
            return StatusCode(StatusCodes.Status200OK, userFromDB);
        }
        catch (Exception ex)
        {
            // Handle and return a service unavailable status with the error message.
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
    }

    /// <summary>
    /// Deletes the specified user.
    /// </summary>
    /// <param name="id">The ID of the user to be deleted.</param>
    /// <param name="user">The user to be deleted.</param>
    /// <returns>
    /// <response code="200">OK - Returns a success message.</response>
    /// <response code="204">No Content - If no user found with the specified ID.</response>
    /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
    /// </returns>
    [HttpDelete]
    [Route("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult Delete(int id)
    {
        // Validate the model state and ID.
        if (id <= 0 || !ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            // Find the user in the database based on the provided ID.
            var userFromDB = _context.Users.Find(id);

            // Check if the user is not found.
            if (userFromDB == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "Location not found.");
            }

            // Remove the user from the database and save changes.
            _context.Users.Remove(userFromDB);
            _context.SaveChanges();

            // Return a JSON result indicating successful deletion.
            return new JsonResult("User deleted successfully.") { StatusCode = StatusCodes.Status200OK };
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
