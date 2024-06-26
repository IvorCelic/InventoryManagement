﻿using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Controller for managing warehouses-related operations within the inventory management system.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WarehouseController : InventoryManagementController<Warehouse, WarehouseDTORead, WarehouseDTOInsertUpdate>
    {
        /// <summary>
        /// Constructor for the WarehouseController.
        /// Initializes the controller with the specified context and sets the DbSet for Warehouses.
        /// </summary>
        /// <param name="context">The InventoryManagementContext for the controller.</param>
        public WarehouseController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.Warehouses;
        }

        /// <summary>
        /// Searches for warehouses with pagination support, allowing for retrieval of warehouse
        /// information in a paginated manner. The search can be filtered by a condition string.
        /// </summary>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="condition">A condition for filtering warehouse names (optional).</param>
        /// <returns>Returns a JSON result containing the paginated list of warehouses matching the condition.</returns>
        [HttpGet]
        [Route("searchPagination/{page}")]
        public IActionResult SearchWarehousePagination(int page, string condition = "")
        {
            var perPage = 8;
            condition = condition.ToLower();

            try
            {
                var warehousesEmpty = new List<Warehouse>();

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
                    return new JsonResult(_mapper.MapReadList(warehousesEmpty));
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Custom logic to control the deletion of a warehouse entity. This method checks if
        /// the warehouse is associated with any inventory transactions. If so, it prevents deletion
        /// and throws an exception with a descriptive message indicating the associated transactions.
        /// </summary>
        /// <param name="entity">The warehouse entity to be deleted.</param>
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
