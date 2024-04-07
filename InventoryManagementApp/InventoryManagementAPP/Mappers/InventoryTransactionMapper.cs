using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    public class InventoryTransactionMapper : Mapping<InventoryTransaction, InventoryTransactionDTORead, InventoryTransactionDTOInsertUpdate>
    {
        public InventoryTransactionMapper()
        {
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
                        entity.AdditionalDetails));
                })
                );


            MapperMapInsertUpdatedFromDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransactionDTOInsertUpdate, InventoryTransaction>();
                })
                );


            MapperMapInsertUpdateToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransaction, InventoryTransactionDTOInsertUpdate>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionDTOInsertUpdate(
                        entity.Employee == null ? null : entity.Employee.Id,
                        entity.TransactionStatus == null ? null : entity.TransactionStatus.Id,
                        entity.TransactionDate,
                        entity.AdditionalDetails));
                })
                );
        }

    }
}