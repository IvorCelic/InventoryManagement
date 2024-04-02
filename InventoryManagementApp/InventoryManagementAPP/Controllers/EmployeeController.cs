using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for Employees entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeController : InventoryManagementController<Employee, EmployeeDTORead, EmployeeDTOInsertUpdate>
    {
        public EmployeeController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.Employees;
        }


        protected override void ControlDelete(Employee entity)
        {
            var list = _context.InventoryTransactions
                .Include(x => x.Employee)
                .Where(x => x.Employee.Id == entity.Id)
                .ToList();

            if (list != null && list.Count > 0)
            {
                throw new Exception("Employee can not be deleted because it is associated with transactions: ");
            }

        }
    }
}
