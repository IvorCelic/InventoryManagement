using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    public class InventoryTransactionItemMapper : Mapping<InventoryTransactionItem, InventoryTransactionItemDTORead, InventoryTransactionDTOInsertUpdate>
    {
        public InventoryTransactionItemMapper()
        {
            MapperMapReadToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransactionItem, InventoryTransactionItemDTORead>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionItemDTORead(
                        entity.Id,
                        entity.InventoryTransaction == null ? null : entity.InventoryTransaction.Id,
                        entity.Warehouse == null ? null : entity.Warehouse.Id,
                        entity.Product == null ? "" : entity.Product.ProductName,
                        entity.Quantity == null ? 1 : entity.Quantity
                        ));
                })
                );


            MapperMapInsertUpdatedFromDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransactionItemDTOInsertUpdate, InventoryTransactionItem>();
                })
                );


            MapperMapInsertUpdateToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransactionItem, InventoryTransactionItemDTOInsertUpdate>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionItemDTOInsertUpdate(
                        entity.InventoryTransaction == null ? null : entity.InventoryTransaction.Id,
                        entity.Warehouse == null ? null : entity.Warehouse.Id,
                        entity.Product == null ? null : entity.Product.Id,
                        entity.Quantity == null ? 1 : entity.Quantity));
                })
                );
        }

    }
}
