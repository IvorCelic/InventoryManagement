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

        /// <summary>
        /// Maps a list of <see cref="Person"/> entities to a list of <see cref="PersonDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="Person"/> entities to map.</param>
        /// <returns>A list of <see cref="PersonDTORead"/> DTOs.</returns>
        public static List<PersonDTORead> MapPersonReadList(this List<Person> list)
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
        /// Maps a <see cref="Person"/> entity to a <see cref="PersonDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Person"/> entity to map.</param>
        /// <returns>A <see cref="PersonDTORead"/> DTO.</returns>
        public static PersonDTORead MapPersonReadToDTO(this Person entity)
        {
            var mapper = PersonMapper.InitializeReadToDTO();

            return mapper.Map<PersonDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="Person"/> entity to a <see cref="PersonDTOInsertUpdate"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Person"/> entity to map.</param>
        /// <returns>A <see cref="PersonDTOInsertUpdate"/> DTO.</returns>
        public static PersonDTOInsertUpdate MapPersonInsertUpdatedToDTO(this Person entity)
        {
            var mapper = PersonMapper.InitializeInsertUpdateToDTO();

            return mapper.Map<PersonDTOInsertUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="PersonDTOInsertUpdate"/> DTO to a <see cref="Person"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="PersonDTOInsertUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="Person"/> entity to update.</param>
        /// <returns>The updated <see cref="Person"/> entity.</returns>
        public static Person MapPersonInsertUpdateFromDTO(this PersonDTOInsertUpdate dto, Person entity)
        {
            entity.FirstName = dto.firstName;
            entity.LastName = dto.lastName;
            entity.Email = dto.email;
            entity.Password = dto.password;

            return entity;
        }
    }
}
