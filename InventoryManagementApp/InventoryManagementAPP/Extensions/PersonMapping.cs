using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Faker;

namespace InventoryManagementAPP.Extensions
{
    /// <summary>
    /// Extension methods for mapping employees between entities and Data Transfer Objects (DTOs) in the inventory management application.
    /// </summary>
    public static class PersonMapping
    {
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
        public static PersonDTOInsertUpdate MapEmployeeInsertUpdatedToDTO(this Person entity)
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

            return entity;
        }
    }
}
