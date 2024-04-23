using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// A mapper class for inventory transaction items, allowing mapping between
    /// InventoryTransactionItem entities and their corresponding Data Transfer Objects (DTOs).
    /// Inherits from the generic Mapping class.
    /// </summary>
    public class InventoryTransactionItemMapper : Mapping<InventoryTransactionItem, InventoryTransactionItemDTORead, InventoryTransactionItemDTOInsertUpdate>
    {
        /// <summary>
        /// Initializes a new instance of the InventoryTransactionItemMapper class.
        /// Configures mappings for converting between InventoryTransactionItem entities
        /// and their corresponding DTOs for read operations and insert/update operations.
        /// </summary>
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
                        entity.Warehouse == null ? "" : entity.Warehouse.WarehouseName,
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
                        entity.Quantity == null ? 1 : entity.Quantity
                        ));
                })
                );
        }

    }
}
