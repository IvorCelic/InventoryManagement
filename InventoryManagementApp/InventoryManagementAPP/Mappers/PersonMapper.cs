using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// Mapper class for configuring AutoMapper profiles to map between Person entities and corresponding Data Transfer Objects (DTOs).
    /// </summary>
    public class PersonMapper
    {
        /// <summary>
        /// Initializes AutoMapper for mapping from Person entities to EmployeeDTORead DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from Person to EmployeeDTORead.</returns>
        public static Mapper InitializeReadToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Person, PersonDTORead>();
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from EmployeeDTORead DTOs to Person entities.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for reading from EmployeeDTORead to Person.</returns>
        public static Mapper InitializeReadFromDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<PersonDTORead, Person>();
                })
                );
        }

        /// <summary>
        /// Initializes AutoMapper for mapping from Person entities to EmployeeDTOInsertUpdate DTOs.
        /// </summary>
        /// <returns>An instance of AutoMapper configured for inserting or updating from Person to EmployeeDTOInsertUpdate.</returns>
        public static Mapper InitializeInsertUpdateToDTO()
        {
            return new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Person, PersonDTOInsertUpdate>();
                })
                );
        }
    }
}
