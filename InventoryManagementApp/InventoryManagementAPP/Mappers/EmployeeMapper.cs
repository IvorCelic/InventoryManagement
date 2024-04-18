using AutoMapper;
using InventoryManagementAPP.Models;

namespace InventoryManagementAPP.Mappers
{
    public class EmployeeMapper : Mapping<Employee, EmployeeDTORead, EmployeeDTOInsertUpdate>
    {
        public EmployeeMapper()
        {
            MapperMapReadToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Employee, EmployeeDTORead>()
                    .ConstructUsing(entity => 
                    new EmployeeDTORead(
                        entity.Id,
                        entity.FirstName,
                        entity.LastName,
                        entity.Email,
                        entity.Password,
                        FilePath(entity)));
                })
                );

            MapperMapInsertUpdateToDTO = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Employee, EmployeeDTOInsertUpdate>()
                    .ConstructUsing(entity => 
                    new EmployeeDTOInsertUpdate(
                        entity.FirstName,
                        entity.LastName,
                        entity.Email,
                        entity.Password,
                        FilePath(entity)));
                })
                );
        }

        private static string FilePath(Employee entity)
        {
            try
            {
                var ds = Path.DirectorySeparatorChar;
                string image = Path.Combine(Directory.GetCurrentDirectory()
                    + ds + "wwwroot" + ds + "images" + ds + "employees" + ds + entity.Id + ".png");

                return File.Exists(image) ? "/images/employees/" + entity.Id + ".png" : null;
            }
            catch
            {
                return null;
            }

        }
    }
}
