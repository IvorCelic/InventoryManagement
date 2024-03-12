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
        /// Maps a list of <see cref="Location"/> entities to a list of <see cref="LocationDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="Location"/> entities to map.</param>
        /// <returns>A list of <see cref="LocationDTORead"/> DTOs.</returns>
        public static List<LocationDTORead> MapLocationReadList(this List<Location> list)
        {
            var mapper = LocationMapper.InitializeReadToDTO();
            var result = new List<LocationDTORead>();
            list.ForEach(entity =>
            {
                result.Add(mapper.Map<LocationDTORead>(entity));
            });

            return result;
        }

        /// <summary>
        /// Maps a <see cref="Location"/> entity to a <see cref="LocationDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Location"/> entity to map.</param>
        /// <returns>A <see cref="LocationDTORead"/> DTO.</returns>
        public static LocationDTORead MapLocationReadToDTO(this Location entity)
        {
            var mapper = LocationMapper.InitializeReadToDTO();

            return mapper.Map<LocationDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="Location"/> entity to a <see cref="LocationDTOInsertUpdate"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Location"/> entity to map.</param>
        /// <returns>A <see cref="LocationDTOInsertUpdate"/> DTO.</returns>
        public static LocationDTOInsertUpdate MapLocationInsertUpdatedToDTO(this Location entity)
        {
            var mapper = LocationMapper.InitializeInsertUpdateToDTO();

            return mapper.Map<LocationDTOInsertUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="LocationDTOInsertUpdate"/> DTO to a <see cref="Location"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="LocationDTOInsertUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="Location"/> entity to update.</param>
        /// <returns>The updated <see cref="Location"/> entity.</returns>
        public static Location MapLocationInsertUpdateFromDTO(this LocationDTOInsertUpdate dto, Location entity)
        {
            entity.Name = dto.name;
            entity.Description = dto.description;

            return entity;
        }
    }
}
