using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for Warehouses entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : InventoryManagementController<Product, ProductDTORead, ProductDTOInsertUpdate>
    {
        public ProductController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.Products;
        }


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
