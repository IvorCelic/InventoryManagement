using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Controller for managing employee-related operations within the inventory management system.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeController : InventoryManagementController<Employee, EmployeeDTORead, EmployeeDTOInsertUpdate>
    {
        private readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing employee data.</param>
        public EmployeeController(InventoryManagementContext context) : base(context)
        {
            _context = context;
            DbSet = _context.Employees;
        }

        /// <summary>
        /// Controls the deletion of an employee entity.
        /// </summary>
        /// <param name="entity">The employee entity to be deleted.</param>
        /// <exception cref="Exception">Thrown when the employee is associated with transactions.</exception>
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
