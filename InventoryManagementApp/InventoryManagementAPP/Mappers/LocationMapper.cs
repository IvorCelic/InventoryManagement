using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// Mapper class for configuring AutoMapper profiles to map between Location entities and corresponding Data Transfer Objects (DTOs).
    /// </summary>
    public class LocationMapper
    {
        /// <summary>
        /// Initializes AutoMapper for mapping from <see cref="Location"/> entities to <see cref="LocationDTORead"/> DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from <see cref="Location"/> to <see cref="LocationDTORead"/>.</returns>
        public static Mapper InitializeReadToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Location, LocationDTORead>();
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from <see cref="LocationDTORead"/> DTOs to <see cref="Location"/> entities.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from <see cref="LocationDTORead"/> to <see cref="Location"/>.</returns>
        public static Mapper InitializeReadFromDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<LocationDTORead, Location>();
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from <see cref="Location"/> entities to <see cref="LocationDTOInsertUpdate"/> DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for inserting or updating from <see cref="Location"/> to <see cref="LocationDTOInsertUpdate"/>.</returns>
        public static Mapper InitializeInsertUpdateToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Location, LocationDTOInsertUpdate>();
                })
                );
        }
    }
}
