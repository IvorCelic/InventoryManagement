using AutoMapper;

namespace InventoryManagementAPP.Mappers
{
    public class Mapping<T, DTR, DTI>
    {

        protected Mapper MapperMapReadToDTO;
        protected Mapper MapperMapInsertUpdatedFromDTO;
        protected Mapper MapperMapInsertUpdateToDTO;

        public Mapping()
        {
            MapperMapReadToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.AllowNullDestinationValues = true;
                    config.CreateMap<T, DTR>();
                })
                );
            MapperMapInsertUpdatedFromDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<DTI, T>();
                })
                );

            MapperMapInsertUpdateToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<T, DTI>();
                })
                );
        }

        public List<DTR> MapReadList(List<T> list)
        {
            var result = new List<DTR>();
            list.ForEach(e =>
            {
                result.Add(MapReadToDTO(e));
            });

            return result;
        }

        public DTR MapReadToDTO(T entity)
        {
            return MapperMapReadToDTO.Map<DTR>(entity);
        }

        public T MapInsertUpdatedFromDTO(DTI entity)
        {
            return MapperMapInsertUpdatedFromDTO.Map<T>(entity);
        }

        public DTI MapInsertUpdateToDTO(T entity)
        {
            return MapperMapInsertUpdateToDTO.Map<DTI>(entity);
        }



    }
}
