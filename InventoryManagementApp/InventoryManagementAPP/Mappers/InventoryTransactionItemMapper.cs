using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    public class InventoryTransactionItemMapper
    {
        public static Mapper InitializeReadToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransactionItem, InventoryTransactionItemDTORead>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionItemDTORead(
                        entity.Id,
                        entity.InventoryTransaction == null ? "" : entity.InventoryTransaction.TransactionStatus.StatusName.Trim(),
                        entity.Product == null ? "" : entity.Product.ProductName.Trim(),
                        entity.Warehouse == null ? "" : entity.Warehouse.WarehouseName.Trim(),
                        entity.Quantity == null ? 0 : entity.Quantity));
                })
                );
        }

        public static Mapper InitializeInsertUpdateToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<InventoryTransactionItem, InventoryTransactionItemDTOInsertUpdate>()
                    .ConstructUsing(entity =>
                    new InventoryTransactionItemDTOInsertUpdate(
                        entity.InventoryTransaction == null ? null : entity.InventoryTransaction.Id,
                        entity.Product == null ? null : entity.Product.Id,
                        entity.Warehouse == null ? null : entity.Warehouse.Id,
                        entity.Quantity));
                })
                );
        }
    }
}
