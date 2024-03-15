using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InventoryManagementAPP.Extensions
{
    /// <summary>
    /// Extension methods for mapping employees between entities and Data Transfer Objects (DTOs) in the inventory management application.
    /// </summary>
    public static class EmployeeMapping
    {
        /// <summary>
        /// Maps a list of <see cref="Employee"/> entities to a list of <see cref="EmployeeDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="Employee"/> entities to map.</param>
        /// <returns>A list of <see cref="EmployeeDTORead"/> DTOs.</returns>
        public static List<EmployeeDTORead> MapEmployeeReadList(this List<Employee> list)
        {
            var mapper = EmployeeMapper.InitializeReadToDTO();
            var result = new List<EmployeeDTORead>();
            list.ForEach(entity =>
            {
                result.Add(mapper.Map<EmployeeDTORead>(entity));
            });

            return result;
        }

        /// <summary>
        /// Maps a <see cref="Employee"/> entity to a <see cref="EmployeeDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Employee"/> entity to map.</param>
        /// <returns>A <see cref="EmployeeDTORead"/> DTO.</returns>
        public static EmployeeDTORead MapEmployeeReadToDTO(this Employee entity)
        {
            var mapper = EmployeeMapper.InitializeReadToDTO();

            return mapper.Map<EmployeeDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="Employee"/> entity to a <see cref="EmployeeDTOInsertUpdate"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Employee"/> entity to map.</param>
        /// <returns>A <see cref="EmployeeDTOInsertUpdate"/> DTO.</returns>
        public static EmployeeDTOInsertUpdate MapEmployeeInsertUpdatedToDTO(this Employee entity)
        {
            var mapper = EmployeeMapper.InitializeInsertUpdateToDTO();

            return mapper.Map<EmployeeDTOInsertUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="EmployeeDTOInsertUpdate"/> DTO to a <see cref="Employee"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="EmployeeDTOInsertUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="Employee"/> entity to update.</param>
        /// <returns>The updated <see cref="Employee"/> entity.</returns>
        public static Employee MapEmployeeInsertUpdateFromDTO(this EmployeeDTOInsertUpdate dto, Employee entity)
        {
            entity.FirstName = dto.firstName;
            entity.LastName = dto.lastName;
            entity.Email = dto.email;
            entity.Password = dto.password;

            return entity;
        }
    }
}
