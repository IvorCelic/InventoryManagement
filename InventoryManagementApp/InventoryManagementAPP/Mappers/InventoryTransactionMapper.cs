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
                        entity.TransactionStatus == null ? "" : entity.TransactionStatus.StatusName.Trim(),
                        entity.TransactionDate,
                        entity.AdditionalDetails));
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from InventoryTransaction entities to InventoryTransactionDTOInsert DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for inserting or updating from InventoryTransaction to InventoryTransactionDTOInsert.</returns>
        public static Mapper InitializeInsertToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransaction, InventoryTransactionDTOInsert>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionDTOInsert(
                        entity.Employee == null ? null : entity.Employee.Id,
                        entity.TransactionStatus == null ? null : entity.TransactionStatus.Id,
                        entity.TransactionDate,
                        entity.AdditionalDetails));
                })
                );
        }

        public static Mapper InitializeUpdateToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransaction, InventoryTransactionDTOUpdate>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionDTOUpdate(
                        entity.Employee == null ? null : entity.Employee.Id,
                        entity.TransactionStatus.Id == 1 ? 2 : 1,
                        entity.AdditionalDetails));
                })
                );
        }
    }
}
