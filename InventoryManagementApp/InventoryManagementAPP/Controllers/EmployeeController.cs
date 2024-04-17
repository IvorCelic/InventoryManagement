using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;

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


        [HttpGet]
        [Route("searchPagination/{page}")]
        public IActionResult SearchEmployeePagination(int page, string condition = "")
        {
            var perPage = 8;
            condition = condition.ToLower();

            try
            {
                var employees = _context.Employees
                    .Where(w => 
                        EF.Functions.Like(w.FirstName.ToLower(), "%" + condition + "%") ||
                        EF.Functions.Like(w.LastName.ToLower(), "%" + condition + "%"))
                    .Skip((perPage * page) - perPage)
                    .Take(perPage)
                    .OrderBy(w => w.LastName)
                    .ToList();

                return new JsonResult(_mapper.MapReadList(employees));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                StringBuilder sb = new StringBuilder();
                sb.Append("Employee can not be deleted because it is associated with transactions: ");

                foreach (var item in list)
                {
                    sb.Append(item.AdditionalDetails).Append(", ");
                }

                throw new Exception(sb.ToString().Substring(0, sb.ToString().Length - 2));
            }
        }
    }
}
