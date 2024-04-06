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
                        entity.InventoryTransaction.TransactionStatus == null ? null : entity.InventoryTransaction.TransactionStatus.StatusName,
                        entity.InventoryTransaction == null ? null : entity.InventoryTransaction.Id,
                        entity.Product == null ? "" : entity.Product.ProductName.Trim(),
                        entity.Warehouse == null ? null : entity.Warehouse.Id,
                        entity.Quantity == null ? 1 : entity.Quantity
                        ));

                    config.CreateMap<InventoryTransactionItem, ProductWithQuantityDTORead>()
                    .ConstructUsing(entity =>
                    new ProductWithQuantityDTORead(
                        entity.Id,
                        entity.Product.ProductName == null ? "" : entity.Product.ProductName.Trim(),
                        entity.Quantity == null ? 1 : entity.Quantity,
                        entity.Product.IsUnitary == null ? null : entity.Product.IsUnitary
                        ));
                })
                );
        }

        public static Mapper InitializeInsertToDTO()
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
                        entity.Quantity == null ? 1 : entity.Quantity));
                })
                );
        }

    }
}
