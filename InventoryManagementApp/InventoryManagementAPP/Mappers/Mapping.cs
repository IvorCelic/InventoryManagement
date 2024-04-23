using AutoMapper;

namespace InventoryManagementAPP.Mappers
{
    /// <summary>
    /// A generic mapping class for mapping between entities and their corresponding Data Transfer Objects (DTOs).
    /// It handles mappings for read-only views, insert/update operations, and vice versa.
    /// </summary>
    /// <typeparam name="T">The source entity type.</typeparam>
    /// <typeparam name="DTR">The read-only Data Transfer Object type.</typeparam>
    /// <typeparam name="DTI">The insert/update Data Transfer Object type.</typeparam>
    public class Mapping<T, DTR, DTI>
    {
        protected Mapper MapperMapReadToDTO;
        protected Mapper MapperMapInsertUpdatedFromDTO;
        protected Mapper MapperMapInsertUpdateToDTO;

        /// <summary>
        /// Initializes the mapper configurations for mapping from the entity to read-only DTOs, from insert/update DTOs to the entity, and from the entity to insert/update DTOs.
        /// </summary>
        public Mapping()
        {
            // Mapping from the entity type (T) to the read-only DTO type (DTR).
            MapperMapReadToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.AllowNullDestinationValues = true; // Allows null values in the mapping destination.
                    config.CreateMap<T, DTR>(); // Maps from T to DTR.
                })
            );

            // Mapping from the insert/update DTO type (DTI) to the entity type (T).
            MapperMapInsertUpdatedFromDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<DTI, T>(); // Maps from DTI to T.
                })
            );

            // Mapping from the entity type (T) to the insert/update DTO type (DTI).
            MapperMapInsertUpdateToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<T, DTI>(); // Maps from T to DTI.
                })
            );
        }

        /// <summary>
        /// Maps a list of entities to a list of read-only DTOs.
        /// </summary>
        /// <param name="list">A list of source entities.</param>
        /// <returns>A list of mapped read-only DTOs.</returns>
        public List<DTR> MapReadList(List<T> list)
        {
            var result = new List<DTR>();
            list.ForEach(e => result.Add(MapReadToDTO(e)));
            return result;
        }

        /// <summary>
        /// Maps a single entity to a read-only DTO.
        /// </summary>
        /// <param name="entity">The source entity to be mapped.</param>
        /// <returns>A mapped read-only DTO.</returns>
        public DTR MapReadToDTO(T entity)
        {
            return MapperMapReadToDTO.Map<DTR>(entity);
        }

        /// <summary>
        /// Maps a single insert/update DTO to its corresponding entity type.
        /// </summary>
        /// <param name="entity">The insert/update DTO to be mapped.</param>
        /// <returns>The mapped entity.</returns>
        public T MapInsertUpdatedFromDTO(DTI entity)
        {
            return MapperMapInsertUpdatedFromDTO.Map<T>(entity);
        }

        /// <summary>
        /// Maps an entity to its corresponding insert/update DTO.
        /// </summary>
        /// <param name="entity">The source entity to be mapped.</param>
        /// <returns>The mapped insert/update DTO.</returns>
        public DTI MapInsertUpdateToDTO(T entity)
        {
            return MapperMapInsertUpdateToDTO.Map<DTI>(entity);
        }
    }
}
