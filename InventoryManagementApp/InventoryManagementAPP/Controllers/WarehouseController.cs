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
    public class WarehouseController : InventoryManagementController<Warehouse, WarehouseDTORead, WarehouseDTOInsertUpdate>
    {
        public WarehouseController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.Warehouses;
        }



        protected override void ControlDelete(Warehouse entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.Warehouse)
                .Where(x => x.Warehouse.Id == entity.Id)
                .ToList();

            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Warehouse can not be deleted because it is associated with transactions: ");

                foreach (var item in list)
                {
                    //sb.Append(item.InventoryTransaction.AdditionalDetails).Append(", ");
                }

                throw new Exception(sb.ToString().Substring(0, sb.ToString().Length - 2));
            }

        }
    }
}
