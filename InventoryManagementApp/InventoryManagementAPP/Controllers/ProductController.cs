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
    public class ProductController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="context">The Inventory Management context for database interaction.</param>
        public ProductController(InventoryManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/Product
        /// </remarks>
        /// <returns>Returns a list of products in the database.</returns>
        /// <response code="200">Success - Returns the list of products.</response>
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
                var products = _context.Products.ToList();

                // Check if locations are found.
                if (products == null || products.Count == 0)
                {
                    // Return a JSON result with a message when no locations are found.
                    return new EmptyResult();
                }

                // Return a JSON result with the list of locations.
                return new JsonResult(products.MapProductReadList());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a product from the database based on the specified ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/Product/{id}
        /// </remarks>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>Returns the requested product if found.</returns>
        /// <response code="200">OK - Returns the requested product.</response>
        /// <response code="204">No Content - If the specified product with the given ID is not found.</response>
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
                // Retrieve the product from the database based on the provided ID.
                var product = _context.Products.Find(id);

                // Check if the product is not found.
                if (product == null)
                {
                    // Return an empty result when the product is not found.
                    return new EmptyResult();
                }

                // Return the found product as a JSON result.
                return new JsonResult(product.MapProductInsertUpdatedToDTO());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST api/v1/Product
        ///     { "name": "example of name", "description": "example description" }
        /// </remarks>
        /// <param name="productDTO">The product to insert in JSON format.</param>
        /// <returns>Returns the newly created product with its ID.</returns>
        /// <response code="201">Created - Returns the newly created product.</response>
        /// <response code="400">Bad request - If the request is invalid or product is null.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Post(ProductDTOInsertUpdate productDTO)
        {
            // Validate the model state and product.
            if (!ModelState.IsValid || productDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var product = productDTO.MapProductInsertUpdateFromDTO(new Product());
                // Add the new product to the database and save changes.
                _context.Products.Add(product);
                _context.SaveChanges();

                // Return a JSON result with the newly created product.
                return StatusCode(StatusCodes.Status201Created, product.MapProductReadToDTO());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Updates the data of an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDTO">The updated product data in JSON format.</param>
        /// <returns>Returns the updated product.</returns>
        /// <response code="200">OK - Returns the updated product.</response>
        /// <response code="400">Bad request - If the request is invalid, product is null, or ID is less than or equal to 0.</response>
        /// <response code="503">Service Unavailable - If the database is not accessible.</response>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Put(int id, ProductDTOInsertUpdate productDTO)
        {
            // Validate the model state, ID, and product.
            if (id <= 0 || !ModelState.IsValid || productDTO == null)
            {
                return BadRequest();
            }

            try
            {
                // Find the existing product in the database based on the provided ID.
                var productFromDB = _context.Products.Find(id);

                // Check if the product is not found.
                if (productFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                var product = productDTO.MapProductInsertUpdateFromDTO(productFromDB);

                _context.Products.Update(productFromDB);
                _context.SaveChanges();

                // Return a JSON result with the updated product.
                return StatusCode(StatusCodes.Status200OK, product.MapProductReadToDTO());
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified product.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>Returns a message indicating successful deletion.</returns>
        /// <response code="200">OK - The product was successfully deleted.</response>
        /// <response code="204">No Content - The specified product was not found.</response>
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
                // Find the product in the database based on the provided ID.
                var productFromDB = _context.Products.Find(id);

                // Check if the product is not found.
                if (productFromDB == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, id);
                }

                // Remove the product from the database and save changes.
                _context.Products.Remove(productFromDB);
                _context.SaveChanges();

                // Return a JSON result indicating successful deletion.
                return new JsonResult(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                // Handle and return a service unavailable status with the error message.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
    }
}
