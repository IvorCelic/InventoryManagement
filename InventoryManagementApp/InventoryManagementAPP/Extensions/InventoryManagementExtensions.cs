using System.Reflection;

namespace EdunovaAPP.Extensions
{
    public static class InventoryManagementExtensions
    {

        public static void AddInventoryManagementSwaggerGen(this IServiceCollection Services)
        {
            Services.AddSwaggerGen(sgo =>
            {
                var o = new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Inventory Management API",
                    Version = "v1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "ivorcelic@gmail.com",
                        Name = "Ivor Ćelić"
                    },
                    Description = "This is documentation for Inventory Management API",
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "Education licence"
                    }

                };
                sgo.SwaggerDoc("v1", o);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                sgo.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            });
        }


        public static void AddInventoryManagementCORS(this IServiceCollection Services)
        {
            Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );

            });
        }
    }
}
