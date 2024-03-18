using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// Mapper class for configuring AutoMapper profiles to map between InventoryTransaction entities and corresponding Data Transfer Objects (DTOs).
    /// </summary>
    public class InventoryTransactionMapper
    {
        /// <summary>
        /// Initializes AutoMapper for mapping from InventoryTransaction entities to InventoryTransactionDTORead DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from InventoryTransaction to InventoryTransactionDTORead.</returns>
        public static Mapper InitializeReadToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransaction, InventoryTransactionDTORead>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionDTORead(
                        entity.Id,
                        entity.Employee == null ? "" : (entity.Employee.FirstName + " " + entity.Employee.LastName).Trim(),
                        entity.TransactionStatus,
                        entity.TransactionDate,
                        entity.AdditionalDetails));
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from InventoryTransaction entities to InventoryTransactionDTOInsertUpdate DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for inserting or updating from InventoryTransaction to InventoryTransactionDTOInsertUpdate.</returns>
        public static Mapper InitializeInsertUpdateToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransaction, InventoryTransactionDTOInsertUpdate>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionDTOInsertUpdate(
                        entity.Employee == null ? null : entity.Employee.Id,
                        entity.TransactionStatus,
                        entity.TransactionDate,
                        entity.AdditionalDetails));
                })
                );
        }
    }
}
