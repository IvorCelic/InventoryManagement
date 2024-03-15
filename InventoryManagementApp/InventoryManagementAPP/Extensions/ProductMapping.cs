using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Extensions
{
    /// <summary>
    /// Extension methods for mapping products between entities and Data Transfer Objects (DTOs) in the inventory management application.
    /// </summary>
    public static class ProductMapping
    {
        /// <summary>
        /// Maps a list of <see cref="Product"/> entities to a list of <see cref="ProductDTORead"/> DTOs.
        /// </summary>
        /// <param name="list">The list of <see cref="Product"/> entities to map.</param>
        /// <returns>A list of <see cref="ProductDTORead"/> DTOs.</returns>
        public static List<ProductDTORead> MapProductReadList(this List<Product> list)
        {
            var mapper = ProductMapper.InitializeReadToDTO();
            var result = new List<ProductDTORead>();
            list.ForEach(entity =>
            {
                result.Add(mapper.Map<ProductDTORead>(entity));
            });

            return result;
        }

        /// <summary>
        /// Maps a <see cref="Product"/> entity to a <see cref="ProductDTORead"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Product"/> entity to map.</param>
        /// <returns>A <see cref="ProductDTORead"/> DTO.</returns>
        public static ProductDTORead MapProductReadToDTO(this Product entity)
        {
            var mapper = ProductMapper.InitializeReadToDTO();

            return mapper.Map<ProductDTORead>(entity);
        }

        /// <summary>
        /// Maps a <see cref="Product"/> entity to a <see cref="ProductDTOInsertUpdate"/> DTO.
        /// </summary>
        /// <param name="entity">The <see cref="Product"/> entity to map.</param>
        /// <returns>A <see cref="ProductDTOInsertUpdate"/> DTO.</returns>
        public static ProductDTOInsertUpdate MapProductInsertUpdatedToDTO(this Product entity)
        {
            var mapper = ProductMapper.InitializeInsertUpdateToDTO();

            return mapper.Map<ProductDTOInsertUpdate>(entity);
        }

        /// <summary>
        /// Maps a <see cref="ProductDTOInsertUpdate"/> DTO to a <see cref="Product"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="ProductDTOInsertUpdate"/> DTO to map.</param>
        /// <param name="entity">The <see cref="Product"/> entity to update.</param>
        /// <returns>The updated <see cref="Product"/> entity.</returns>
        public static Product MapProductInsertUpdateFromDTO(this ProductDTOInsertUpdate dto, Product entity)
        {
            entity.ProductName = dto.productName;
            entity.Description = dto.description;
            entity.IsUnitary = dto.isUnitary;

            return entity;
        }
    }
}
