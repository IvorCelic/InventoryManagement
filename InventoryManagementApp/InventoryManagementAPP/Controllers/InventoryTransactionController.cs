using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPP.Mappers;

namespace InventoryManagementAPP.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionController : InventoryManagementController<InventoryTransaction, InventoryTransactionDTORead, InventoryTransactionDTOInsertUpdate>
    {

        public InventoryTransactionController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.InventoryTransactions;
            _mapper = new InventoryTransactionMapper();
        }


        protected override InventoryTransaction EditEntity(InventoryTransactionDTOInsertUpdate entityDTO, InventoryTransaction entity)
        {
            var employee = _context.Employees.Find(entityDTO.employeeId) ?? throw new Exception("There is no Employee with ID: " + entityDTO.employeeId + " in database.");
            var transactionStatus = _context.TransactionStatuses.Find(entityDTO.transactionStatusId) ?? throw new Exception("There is no Transaction Status with ID: " + entityDTO.transactionStatusId + " in database.");

            entity.Employee = employee;
            entity.TransactionStatus = transactionStatus;
            entity.TransactionDate = entityDTO.transactionDate;
            entity.AdditionalDetails = entityDTO.additionalDetails;

            return entity;
        }


        protected override InventoryTransaction LoadEntity(int id)
        {
            var entity = _context.InventoryTransactions
                .Include(it => it.Employee)
                .Include(it => it.TransactionStatus)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new Exception("There is no Inventory Transaction with ID: " + id + " in database.");
            }

            return entity;
        }


        protected override List<InventoryTransactionDTORead> LoadEntites()
        {
            var list = _context.InventoryTransactions
                .Include(it => it.Employee)
                .Include(it => it.TransactionStatus)
                .ToList();

            if (list == null || list.Count == 0)
            {
                throw new Exception("No data in database.");
            }

            return _mapper.MapReadList(list);
        }


        protected override InventoryTransaction CreateEntity(InventoryTransactionDTOInsertUpdate entityDTO)
        {
            var employee = _context.Employees.Find(entityDTO.employeeId);
            if (employee == null)
            {
                throw new Exception("There is no Employee with ID: " + employee.Id + " in database.");
            }

            var transactionStatus = _context.TransactionStatuses.Find(entityDTO.transactionStatusId);
            if (transactionStatus == null)
            {
                throw new Exception("There is no Transaction Status with ID: " + transactionStatus.Id + " in database.");
            }

            var entity = _mapper.MapInsertUpdatedFromDTO(entityDTO);
            entity.Employee = employee;
            entity.TransactionStatus = transactionStatus;

            return entity;
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
