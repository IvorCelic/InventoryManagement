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


        [HttpGet]
        [Route("searchPagination/{page}")]
        public IActionResult SearchWarehousePagination(int page, string condition = "")
        {
            var perPage = 8;
            condition = condition.ToLower();

            try
            {
                var warehouses = _context.Warehouses
                    .Where(w => EF.Functions.Like(w.WarehouseName.ToLower(), "%" + condition + "%"))
                    .Skip((perPage * page) - perPage)
                    .Take(perPage)
                    .OrderBy(w => w.WarehouseName)
                    .ToList();

                return new JsonResult(_mapper.MapReadList(warehouses));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        protected override void ControlDelete(Warehouse entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.Warehouse)
                .Include(it => it.InventoryTransaction)
                .Where(x => x.Warehouse.Id == entity.Id)
                .Select(x => x.InventoryTransaction)
                .Distinct()
                .ToList();

            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Warehouse can not be deleted because it is associated with transactions: ");

                foreach (var item in list)
                {
                    sb.Append(item.AdditionalDetails).Append(", ");
                }

                throw new Exception(sb.ToString().Substring(0, sb.ToString().Length - 2));
            }

        }
    }
}
