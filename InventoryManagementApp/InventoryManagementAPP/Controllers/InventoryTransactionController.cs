using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionController : InventoryManagementController<InventoryTransaction, InventoryTransactionDTORead, InventoryTransactionDTOInsertUpdate>
    {

        public InventoryTransactionController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.InventoryTransactions;
        }

        protected override void ControlDelete(InventoryTransaction entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.InventoryTransaction)
                .Where(x => x.InventoryTransaction.Id == entity.Id)
                .ToList();

            if (list != null && list.Count() > 0)
            {
                throw new Exception("Transaction can not be deleted because it contains InventoryTransactionItems on it. ");
            }
        }

    }
}
