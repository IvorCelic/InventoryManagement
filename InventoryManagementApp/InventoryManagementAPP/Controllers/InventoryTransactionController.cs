using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPP.Mappers;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Controller for managing InventoryTransactions-related operations within the inventory management system.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryTransactionController : InventoryManagementController<InventoryTransaction, InventoryTransactionDTORead, InventoryTransactionDTOInsertUpdate>
    {
        /// <summary>
        /// Initializes a new instance of the InventoryTransactionController class.
        /// </summary>
        /// <param name="context">The database context for inventory management.</param>
        public InventoryTransactionController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.InventoryTransactions;
            _mapper = new InventoryTransactionMapper();
        }

        /// <summary>
        /// Edits an existing InventoryTransaction entity with new data from a DTO.
        /// This method also ensures that the associated employee and transaction status exist in the database.
        /// </summary>
        /// <param name="entityDTO">The data transfer object containing the updated data.</param>
        /// <param name="entity">The existing InventoryTransaction entity to be edited.</param>
        /// <returns>The updated InventoryTransaction entity.</returns>
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

        /// <summary>
        /// Loads an InventoryTransaction entity by its ID, including associated Employee and TransactionStatus data.
        /// Throws an exception if no transaction is found with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the InventoryTransaction to load.</param>
        /// <returns>The InventoryTransaction entity with the specified ID.</returns>
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

        /// <summary>
        /// Loads all InventoryTransaction entities from the database, including associated Employee and TransactionStatus data.
        /// Throws an exception if no data is found.
        /// </summary>
        /// <returns>A list of InventoryTransactionDTORead containing all transactions.</returns>
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

        /// <summary>
        /// Creates a new InventoryTransaction entity from a DTO, ensuring that the associated Employee and TransactionStatus exist in the database.
        /// </summary>
        /// <param name="entityDTO">The data transfer object containing the transaction data.</param>
        /// <returns>The created InventoryTransaction entity.</returns>
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

        /// <summary>
        /// Custom logic to control the deletion of an InventoryTransaction entity.
        /// </summary>
        /// <param name="entity">The InventoryTransaction entity to be deleted.</param>
        protected override void ControlDelete(InventoryTransaction entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.InventoryTransaction)
                .Where(x => x.InventoryTransaction.Id == entity.Id)
                .ToList();

            if (list != null && list.Count() > 0)
            {
                throw new Exception("Transaction can not be deleted because it contains items on it. ");
            }
        }

    }
}
