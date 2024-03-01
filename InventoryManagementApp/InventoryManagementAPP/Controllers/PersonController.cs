using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Controller for performing CRUD operations on the Persons entity in the database.
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
        /// Retrieves all persons from the database.
        /// </summary>
        /// <returns>
        /// <response code="200">Success - Returns the list of persons.</response>
        /// <response code="204">No Content - If no persons found.</response>
        /// <response code="400">Bad Request - If the request is invalid.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Person>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return new JsonResult(persons);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to be created.</param>
        /// <returns>
        /// <response code="201">Created - Returns the created person.</response>
        /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(Person person)
        {
            if (!ModelState.IsValid || person == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Persons.Add(person);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, person);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Updates the data of an existing person.
        /// </summary>
        /// <param name="id">The ID of the person to be updated.</param>
        /// <param name="person">The updated person information.</param>
        /// <returns>
        /// <response code="200">OK - Returns the updated person.</response>
        /// <response code="400">Bad Request - If the request is invalid or missing required fields.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Put(int id, Person person)
        {
            if (id <= 0 || !ModelState.IsValid || person == null)
            {
                return BadRequest();
            }

            try
            {
                var locationFromDB = _context.Persons.Find(id);

                if (locationFromDB == null)
                {
                    return BadRequest();
                }

                // inače ovo rade mapperi
                // za sada ručno
                locationFromDB.FirstName = person.FirstName;
                locationFromDB.LastName = person.LastName;
                locationFromDB.Email = person.Email;
                locationFromDB.Password = person.Password;

                _context.Persons.Update(locationFromDB);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, locationFromDB);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified person.
        /// </summary>
        /// <param name="id">The ID of the person to be deleted.</param>
        /// <param name="person">The person to be deleted.</param>
        /// <returns>
        /// <response code="200">OK - Returns a success message.</response>
        /// <response code="204">No Content - If no person found with the specified ID.</response>
        /// <response code="503">Service Unavailable - If an unexpected error occurs.</response>
        /// </returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Delete(int id, Person person)
        {
            if (id <= 0 || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var smjerIzBaze = _context.Persons.Find(id);

                if (smjerIzBaze == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                _context.Persons.Remove(smjerIzBaze);
                _context.SaveChanges();

                return new JsonResult("Person deleted.");
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
