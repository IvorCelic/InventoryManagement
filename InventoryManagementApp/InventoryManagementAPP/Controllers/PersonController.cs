using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for Employees entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public PersonController(InventoryManagementContext context)
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
                var persons = _context.Persons.ToList();

                if (persons == null || persons.Count == 0)
                {
                    return new EmptyResult();
                }

                return new JsonResult(persons.MapPersonReadList());
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
                var person = _context.Persons.Find(id);

                if (person == null)
                {
                    return new EmptyResult();
                }

                return new JsonResult(person.MapEmployeeInsertUpdatedToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="personDTO">The employee to be created.</param>
        /// <returns>
        /// <response code="201">Created - Returns the created employee.</response>
        /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(PersonDTOInsertUpdate personDTO)
        {
            if (!ModelState.IsValid || personDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var person = personDTO.MapPersonInsertUpdateFromDTO(new Person());
                _context.Persons.Add(person);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, person.MapPersonReadToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        [HttpPost]
        [Route("test")]
        public IActionResult PostExact(int number, PersonDTOInsertUpdate personDTO)
        {
            if (!ModelState.IsValid || personDTO == null)
            {
                return BadRequest();
            }

            try
            {
                for (int i = 0; i <= number; i++)
                {
                    var fakePerson = new Person
                    {
                        FirstName = Faker.Name.First(),
                        LastName = Faker.Name.Last(),
                        Email = Faker.Internet.Email()
                    };

                    _context.Persons.Add(fakePerson);
                }

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
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
        /// <param name="personDTO">The updated employee information.</param>
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
        public IActionResult Put(int id, PersonDTOInsertUpdate personDTO)
        {
            if (id <= 0 || !ModelState.IsValid || personDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var personFromDB = _context.Persons.Find(id);

                if (personFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                var persons = personDTO.MapPersonInsertUpdateFromDTO(personFromDB);

                _context.Persons.Update(persons);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, persons.MapPersonReadToDTO());
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
                var personFromDB = _context.Persons.Find(id);

                if (personFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                _context.Persons.Remove(personFromDB);
                _context.SaveChanges();

                return new JsonResult(new { message = "Person deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

    }
}
