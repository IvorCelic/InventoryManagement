using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// Mapper class for configuring AutoMapper profiles to map between Product entities and corresponding Data Transfer Objects (DTOs).
    /// </summary>
    public class ProductMapper
    {
        /// <summary>
        /// Initializes AutoMapper for mapping from Product entities to ProductDTORead DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from Product to ProductDTORead.</returns>
        public static Mapper InitializeReadToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Product, ProductDTORead>();
                })
                );
        }

        /// <summary>s
        /// Initializes AutoMapper for mapping from ProductDTORead DTOs to Product entities.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from ProductDTORead to Product.</returns>
        public static Mapper InitializeReadFromDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<ProductDTORead, Product>();
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from Product entities to ProductDTOInsertUpdate DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for inserting or updating from Product to ProductDTOInsertUpdate.</returns>
        public static Mapper InitializeInsertUpdateToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Product, ProductDTOInsertUpdate>();
                })
                );
        }
    }
}
