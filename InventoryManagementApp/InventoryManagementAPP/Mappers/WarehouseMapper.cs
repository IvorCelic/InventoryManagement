using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// Mapper class for configuring AutoMapper profiles to map between Location entities and corresponding Data Transfer Objects (DTOs).
    /// </summary>
    public class WarehouseMapper
    {
        /// <summary>
        /// Initializes AutoMapper for mapping from <see cref="Warehouse"/> entities to <see cref="WarehouseDTORead"/> DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from <see cref="Warehouse"/> to <see cref="WarehouseDTORead"/>.</returns>
        public static Mapper InitializeReadToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Warehouse, WarehouseDTORead>();
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from <see cref="WarehouseDTORead"/> DTOs to <see cref="Warehouse"/> entities.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from <see cref="WarehouseDTORead"/> to <see cref="Warehouse"/>.</returns>
        public static Mapper InitializeReadFromDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<WarehouseDTORead, Warehouse>();
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from <see cref="Warehouse"/> entities to <see cref="WarehouseDTOInsertUpdate"/> DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for inserting or updating from <see cref="Warehouse"/> to <see cref="WarehouseDTOInsertUpdate"/>.</returns>
        public static Mapper InitializeInsertUpdateToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Warehouse, WarehouseDTOInsertUpdate>();
                })
                );
        }
    }
}
