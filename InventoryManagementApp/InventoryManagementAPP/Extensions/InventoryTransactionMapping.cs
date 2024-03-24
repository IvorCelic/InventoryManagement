using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Extensions
{
    /// <summary>
    /// Extension methods for mapping inventorytransactions between entities and Data Transfer Objects (DTOs) in the inventory management application.
    /// </summary>
    public static class InventoryTransactionMapping
    {
        /// <summary>
        /// Maps a list of <see cref="InventoryTransaction"/> entities to a list of <see cref="InventoryTransactionDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="InventoryTransaction"/> entities to map.</param>
        /// <returns>A list of <see cref="InventoryTransactionDTORead"/> DTOs.</returns>
        public static List<InventoryTransactionDTORead> MapInventoryTransactionReadList(this List<InventoryTransaction> list)
        {
            var mapper = InventoryTransactionMapper.InitializeReadToDTO();
            var result = new List<InventoryTransactionDTORead>();
            list.ForEach(entity =>
            {
                result.Add(mapper.Map<InventoryTransactionDTORead>(entity));
            });

            return result;
        }

        /// <summary>
        /// Maps a <see cref="InventoryTransaction"/> entity to a <see cref="InventoryTransactionDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="InventoryTransaction"/> entity to map.</param>
        /// <returns>A <see cref="InventoryTransactionDTORead"/> DTO.</returns>
        public static InventoryTransactionDTORead MapInventoryTransactionReadToDTO(this InventoryTransaction entity)
        {
            var mapper = InventoryTransactionMapper.InitializeReadToDTO();

            return mapper.Map<InventoryTransactionDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="InventoryTransaction"/> entity to a <see cref="InventoryTransactionDTOInsert"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="InventoryTransaction"/> entity to map.</param>
        /// <returns>A <see cref="InventoryTransactionDTOInsert"/> DTO.</returns>
        public static InventoryTransactionDTOInsert MapInventoryTransactionInsertToDTO(this InventoryTransaction entity)
        {
            var mapper = InventoryTransactionMapper.InitializeInsertToDTO();

            return mapper.Map<InventoryTransactionDTOInsert>(entity);
        }

        /// <summary>
        /// Maps a <see cref="InventoryTransactionDTOInsert"/> DTO to a <see cref="InventoryTransaction"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="InventoryTransactionDTOInsert"/> DTO to map.</param>
        /// <param name="entity">The <see cref="InventoryTransaction"/> entity to update.</param>
        /// <returns>The updated <see cref="InventoryTransaction"/> entity.</returns>
        public static InventoryTransaction MapInventoryTransactionInsertFromDTO(this InventoryTransactionDTOInsert dto, InventoryTransaction entity)
        {
            entity.TransactionDate = dto.transactionDate;
            entity.AdditionalDetails = dto.additionalDetails;

            return entity;
        }

        public static InventoryTransactionDTOUpdate MapInventoryTransactionUpdateToDTO(this InventoryTransaction entity)
        {
            var mapper = InventoryTransactionMapper.InitializeUpdateToDTO();

            return mapper.Map<InventoryTransactionDTOUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="InventoryTransactionDTOUpdate"/> DTO to a <see cref="InventoryTransaction"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="InventoryTransactionDTOUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="InventoryTransaction"/> entity to update.</param>
        /// <returns>The updated <see cref="InventoryTransaction"/> entity.</returns>
        public static InventoryTransaction MapInventoryTransactionUpdateFromDTO(this InventoryTransactionDTOUpdate dto, InventoryTransaction entity)
        {
            return entity;
        }
    }
}
