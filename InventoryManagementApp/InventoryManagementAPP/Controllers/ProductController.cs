using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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



        protected override void ControlDelete(Product entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.Product)
                .Where(x => x.Product.Id == entity.Id)
                .ToList();

            if (list != null && list.Count > 0)
            {
                throw new Exception("Product can not be deleted because it is associated with transactions: ");
            }

        }

    }
}
