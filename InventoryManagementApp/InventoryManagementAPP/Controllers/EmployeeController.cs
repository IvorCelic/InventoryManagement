using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for Employees entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public EmployeeController(InventoryManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all employees from the database.
        /// </summary>
        /// <returns>
        /// <response code="200">Success - Returns the list of employees.</response>
        /// <response code="204">No Content - If no employees found.</response>
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
                var employees = _context.Employees.ToList();

                if (employees == null || employees.Count == 0)
                {
                    return new EmptyResult();
                }

                return new JsonResult(employees.MapEmployeeReadList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a employee from the database based on the specified ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/Employee/{id}
        /// </remarks>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>Returns the requested employee if found.</returns>
        /// <response code="200">OK - Returns the requested employee.</response>
        /// <response code="204">No Content - If the specified employee with the given ID is not found.</response>
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
                var employee = _context.Employees.Find(id);

                if (employee == null)
                {
                    return new EmptyResult();
                }

                return new JsonResult(employee.MapEmployeeInsertUpdatedToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employeeDTO">The employee to be created.</param>
        /// <returns>
        /// <response code="201">Created - Returns the created employee.</response>
        /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(EmployeeDTOInsertUpdate employeeDTO)
        {
            if (!ModelState.IsValid || employeeDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var person = employeeDTO.MapEmployeeInsertUpdateFromDTO(new Employee());
                _context.Employees.Add(person);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, person.MapEmployeeReadToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Updates the data of an existing employee.
        /// </summary>
        /// <param name="id">The ID of the employee to be updated.</param>
        /// <param name="employeeDTO">The updated employee information.</param>
        /// <returns>
        /// <response code="200">OK - Returns the updated employee.</response>
        /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
        /// <response code="204">No Content - If the specified employee with the given ID is not found.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Put(int id, EmployeeDTOInsertUpdate employeeDTO)
        {
            if (id <= 0 || !ModelState.IsValid || employeeDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var employeeFromDB = _context.Employees.Find(id);

                if (employeeFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                var employee = employeeDTO.MapEmployeeInsertUpdateFromDTO(employeeFromDB);

                _context.Employees.Update(employee);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, employee.MapEmployeeReadToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified employee.
        /// </summary>
        /// <param name="id">The ID of the employee to be deleted.</param>
        /// <returns>
        /// <response code="200">OK - Returns a success message.</response>
        /// <response code="204">No Content - If no employee found with the specified ID.</response>
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
                var employeeFromDB = _context.Employees.Find(id);

                if (employeeFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                _context.Employees.Remove(employeeFromDB);
                _context.SaveChanges();

                return new JsonResult(new { message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

    }
}
