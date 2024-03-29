﻿using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Extensions
{
    /// <summary>
    /// Extension methods for mapping inventorytransactionitems between entities and Data Transfer Objects (DTOs) in the inventory management application.
    /// </summary>
    public static class InventoryTransactionItemMapping
    {
        /// <summary>
        /// Maps a list of <see cref="InventoryTransactionItem"/> entities to a list of <see cref="InventoryTransactionItemDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="InventoryTransactionItem"/> entities to map.</param>
        /// <returns>A list of <see cref="InventoryTransactionItemDTORead"/> DTOs.</returns>
        public static List<InventoryTransactionItemDTORead> MapInventoryTransactionItemReadList(this List<InventoryTransactionItem> list)
        {
            var mapper = InventoryTransactionItemMapper.InitializeReadToDTO();
            var result = new List<InventoryTransactionItemDTORead>();
            list.ForEach(entity =>
            {
                result.Add(mapper.Map<InventoryTransactionItemDTORead>(entity));
            });

            return result;
        }

        /// <summary>
        /// Maps a <see cref="InventoryTransactionItem"/> entity to a <see cref="InventoryTransactionItemDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="InventoryTransactionItem"/> entity to map.</param>
        /// <returns>A <see cref="InventoryTransactionItemDTORead"/> DTO.</returns>
        public static InventoryTransactionItemDTORead MapInventoryTransactionItemReadToDTO(this InventoryTransactionItem entity)
        {
            var mapper = InventoryTransactionItemMapper.InitializeReadToDTO();

            return mapper.Map<InventoryTransactionItemDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="InventoryTransactionItem"/> entity to a <see cref="InventoryTransactionItemDTOInsertUpdate"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="InventoryTransactionItem"/> entity to map.</param>
        /// <returns>A <see cref="InventoryTransactionItemDTOInsertUpdate"/> DTO.</returns>
        public static InventoryTransactionItemDTOInsertUpdate MapInventoryTransactionItemInsertUpdatedToDTO(this InventoryTransactionItem entity)
        {
            var mapper = InventoryTransactionItemMapper.InitializeInsertUpdateToDTO();

            return mapper.Map<InventoryTransactionItemDTOInsertUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="InventoryTransactionItemDTOInsertUpdate"/> DTO to a <see cref="InventoryTransactionItem"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="InventoryTransactionItemDTOInsertUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="InventoryTransactionItem"/> entity to update.</param>
        /// <returns>The updated <see cref="InventoryTransactionItem"/> entity.</returns>
        public static InventoryTransactionItem MapInventoryTransactionItemInsertUpdateFromDTO(this InventoryTransactionItemDTOInsertUpdate dto, InventoryTransactionItem entity)
        {
            entity.Quantity = dto.quantity;

            return entity;
        }
    }
}
