using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Controller for managing products-related operations within the inventory management system.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : InventoryManagementController<Product, ProductDTORead, ProductDTOInsertUpdate>
    {
        /// <summary>
        /// Constructor for the ProductController.
        /// Initializes the controller with the specified context and sets the DbSet for Products.
        /// </summary>
        /// <param name="context">The InventoryManagementContext for the controller.</param>
        public ProductController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.Products;
        }

        /// <summary>
        /// Searches for products with pagination support, allowing for retrieval of product
        /// information in a paginated manner. The search can be filtered by a condition string.
        /// </summary>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="condition">A condition for filtering product names (optional).</param>
        /// <returns>Returns a JSON result containing the paginated list of products matching the condition.</returns>
        [HttpGet]
        [HttpGet]
        [Route("searchPagination/{page}")]
        public IActionResult SearchProductPagination(int page, string condition = "")
        {
            var perPage = 8;
            condition = condition.ToLower();

            try
            {

                var productsEmpty = new List<Product>();

                try
                {

                    var products = _context.Products
                        .Where(p => EF.Functions.Like(p.ProductName.ToLower(), "%" + condition + "%"))
                        .Skip((perPage * page) - perPage)
                        .Take(perPage)
                        .OrderBy(p => p.ProductName)
                        .ToList();

                    return new JsonResult(_mapper.MapReadList(products));
                }
                catch (Exception ex)
                {
                    return new JsonResult(_mapper.MapReadList(productsEmpty));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Custom logic to control the deletion of a product entity. This method checks if
        /// the product is associated with any inventory transactions. If so, it prevents deletion
        /// and throws an exception with a descriptive message indicating the associated transactions.
        /// </summary>
        /// <param name="entity">The product entity to be deleted.</param>
        protected override void ControlDelete(Product entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.Product)
                .Include(it => it.InventoryTransaction)
                .Where(x => x.Product.Id == entity.Id)
                .Select(x => x.InventoryTransaction)
                .Distinct()
                .ToList();

            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Product can not be deleted because it is associated with transactions: ");

                foreach (var item in list)
                {
                    sb.Append(item.AdditionalDetails).Append(", ");
                }

                throw new Exception(sb.ToString().Substring(0, sb.ToString().Length - 2));
            }

        }

    }
}
