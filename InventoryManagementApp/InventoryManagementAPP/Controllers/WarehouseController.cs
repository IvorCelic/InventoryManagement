using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for Warehouses entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WarehouseController : InventoryManagementController<Warehouse, WarehouseDTORead, WarehouseDTOInsertUpdate>
    {
        public WarehouseController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.Warehouses;
        }



        protected override void ControlDelete(Warehouse entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.Product)
                .Where(x => x.Product.Id == entity.Id)
                .ToList();

            if (list != null && list.Count > 0)
            {
                throw new Exception("Warehouse can not be deleted because it is associated with transactions: ");
            }

        }
    }
}
