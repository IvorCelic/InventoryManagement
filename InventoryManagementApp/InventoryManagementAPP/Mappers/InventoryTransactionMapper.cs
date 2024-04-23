using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// Mapper class for InventoryTransaction, providing customized mappings between InventoryTransaction entities and their associated Data Transfer Objects (DTOs).
    /// Inherits from the generic Mapping class with specific types.
    /// </summary>
    public class InventoryTransactionMapper : Mapping<InventoryTransaction, InventoryTransactionDTORead, InventoryTransactionDTOInsertUpdate>
    {
        /// <summary>
        /// Initializes the mapping configurations for InventoryTransaction. Includes specific mapping rules for read-only DTOs and insert/update DTOs.
        /// </summary>
        public InventoryTransactionMapper()
        {
            // Maps from InventoryTransaction to InventoryTransactionDTORead with specific data transformation.
            MapperMapReadToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransaction, InventoryTransactionDTORead>()
                    .ConstructUsing(entity =>
                        new InventoryTransactionDTORead(
                            entity.Id,
                            entity.Employee == null ? "" : (entity.Employee.FirstName + " " + entity.Employee.LastName).Trim(),
                            entity.TransactionStatus == null ? "" : entity.TransactionStatus.StatusName,
                            entity.TransactionDate,
                            entity.AdditionalDetails
                        )
                    );
                })
            );

            // Maps from InventoryTransactionDTOInsertUpdate to InventoryTransaction.
            MapperMapInsertUpdatedFromDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransactionDTOInsertUpdate, InventoryTransaction>();
                })
            );

            // Maps from InventoryTransaction to InventoryTransactionDTOInsertUpdate with specific data transformation.
            MapperMapInsertUpdateToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransaction, InventoryTransactionDTOInsertUpdate>()
                    .ConstructUsing(entity =>
                        new InventoryTransactionDTOInsertUpdate(
                            entity.Employee == null ? null : entity.Employee.Id,
                            entity.TransactionStatus == null ? null : entity.TransactionStatus.Id,
                            entity.TransactionDate,
                            entity.AdditionalDetails
                        )
                    );
                })
            );
        }
    }
}
