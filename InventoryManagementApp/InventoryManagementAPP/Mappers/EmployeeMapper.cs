using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// Mapper class for Employee, providing mappings between Employee entities and their corresponding Data Transfer Objects (DTOs).
    /// Inherits from the generic Mapping class with specific types for Employee-related DTOs.
    /// </summary>
    public class EmployeeMapper : Mapping<Employee, EmployeeDTORead, EmployeeDTOInsertUpdate>
    {
        /// <summary>
        /// Initializes the mapping configurations for Employee entities. Includes specific logic for mapping between Employee entities and their related read-only and insert/update DTOs.
        /// </summary>
        public EmployeeMapper()
        {
            // Maps from Employee to EmployeeDTORead with specific handling for image file paths.
            MapperMapReadToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Employee, EmployeeDTORead>()
                    .ConstructUsing(entity =>
                        new EmployeeDTORead(
                            entity.Id,
                            entity.FirstName,
                            entity.LastName,
                            entity.Email,
                            entity.Password,
                            FilePath(entity) // Uses a helper method to determine the image path.
                        )
                    );
                })
            );

            // Maps from Employee to EmployeeDTOInsertUpdate with specific handling for image file paths.
            MapperMapInsertUpdateToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Employee, EmployeeDTOInsertUpdate>()
                    .ConstructUsing(entity =>
                        new EmployeeDTOInsertUpdate(
                            entity.FirstName,
                            entity.LastName,
                            entity.Email,
                            entity.Password,
                            FilePath(entity) // Determines the image path for employee.
                        )
                    );
                })
            );
        }

        /// <summary>
        /// Helper method to determine the file path for an employee's image.
        /// Checks if the employee's image file exists and returns the relative path if it does.
        /// </summary>
        /// <param name="entity">The Employee entity to check.</param>
        /// <returns>The relative path to the employee's image or null if it doesn't exist.</returns>
        private static string FilePath(Employee entity)
        {
            try
            {
                var ds = Path.DirectorySeparatorChar; // Get the directory separator character for the current system.
                string image = Path.Combine(
                    Directory.GetCurrentDirectory()
                    + ds + "wwwroot" + ds + "images" + ds + "employees" + ds + entity.Id + ".png"
                );

                return File.Exists(image) ? "/images/employees/" + entity.Id + ".png" : null; // Return the relative path if the file exists.
            }
            catch
            {
                return null; // If any exception occurs, return null.
            }
        }
    }
}
