using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InventoryManagementAPP.Extensions
{
    /// <summary>
    /// Extension methods for mapping between entities and Data Transfer Objects (DTOs) in the inventory management application.
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        /// Maps a list of <see cref="Warehouse"/> entities to a list of <see cref="WarehouseDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="Warehouse"/> entities to map.</param>
        /// <returns>A list of <see cref="WarehouseDTORead"/> DTOs.</returns>
        public static List<WarehouseDTORead> MapWarehouseReadList(this List<Warehouse> list)
        {
            var mapper = WarehouseMapper.InitializeReadToDTO();
            var result = new List<WarehouseDTORead>();
            list.ForEach(entity =>
            {
                result.Add(mapper.Map<WarehouseDTORead>(entity));
            });

            return result;
        }

        /// <summary>
        /// Maps a <see cref="Warehouse"/> entity to a <see cref="WarehouseDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Warehouse"/> entity to map.</param>
        /// <returns>A <see cref="WarehouseDTORead"/> DTO.</returns>
        public static WarehouseDTORead MapWarehouseReadToDTO(this Warehouse entity)
        {
            var mapper = WarehouseMapper.InitializeReadToDTO();

            return mapper.Map<WarehouseDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="Warehouse"/> entity to a <see cref="WarehouseDTOInsertUpdate"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Warehouse"/> entity to map.</param>
        /// <returns>A <see cref="WarehouseDTOInsertUpdate"/> DTO.</returns>
        public static WarehouseDTOInsertUpdate MapWarehouseInsertUpdatedToDTO(this Warehouse entity)
        {
            var mapper = WarehouseMapper.InitializeInsertUpdateToDTO();

            return mapper.Map<WarehouseDTOInsertUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="WarehouseDTOInsertUpdate"/> DTO to a <see cref="Warehouse"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="WarehouseDTOInsertUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="Warehouse"/> entity to update.</param>
        /// <returns>The updated <see cref="Warehouse"/> entity.</returns>
        public static Warehouse MapWarehouseInsertUpdateFromDTO(this WarehouseDTOInsertUpdate dto, Warehouse entity)
        {
            entity.WarehouseName = dto.warehouseName;
            entity.Description = dto.description;

            return entity;
        }

        /// <summary>
        /// Maps a list of <see cref="Employee"/> entities to a list of <see cref="PersonDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="Employee"/> entities to map.</param>
        /// <returns>A list of <see cref="PersonDTORead"/> DTOs.</returns>
        public static List<PersonDTORead> MapPersonReadList(this List<Employee> list)
        {
            var mapper = PersonMapper.InitializeReadToDTO();
            var result = new List<PersonDTORead>();
            list.ForEach(entity =>
            {
                result.Add(mapper.Map<PersonDTORead>(entity));
            });

            return result;
        }

        /// <summary>
        /// Maps a <see cref="Employee"/> entity to a <see cref="PersonDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Employee"/> entity to map.</param>
        /// <returns>A <see cref="PersonDTORead"/> DTO.</returns>
        public static PersonDTORead MapPersonReadToDTO(this Employee entity)
        {
            var mapper = PersonMapper.InitializeReadToDTO();

            return mapper.Map<PersonDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="Employee"/> entity to a <see cref="PersonDTOInsertUpdate"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Employee"/> entity to map.</param>
        /// <returns>A <see cref="PersonDTOInsertUpdate"/> DTO.</returns>
        public static PersonDTOInsertUpdate MapPersonInsertUpdatedToDTO(this Employee entity)
        {
            var mapper = PersonMapper.InitializeInsertUpdateToDTO();

            return mapper.Map<PersonDTOInsertUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="PersonDTOInsertUpdate"/> DTO to a <see cref="Employee"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="PersonDTOInsertUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="Employee"/> entity to update.</param>
        /// <returns>The updated <see cref="Employee"/> entity.</returns>
        public static Employee MapPersonInsertUpdateFromDTO(this PersonDTOInsertUpdate dto, Employee entity)
        {
            entity.FirstName = dto.firstName;
            entity.LastName = dto.lastName;
            entity.Email = dto.email;
            entity.Password = dto.password;

            return entity;
        }
    }
}
