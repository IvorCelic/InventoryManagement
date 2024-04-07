using Microsoft.OpenApi.Models;
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



                // Security
                sgo.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization: first on route '/api/b1/Authorization/token' authorize.
                                    Write 'Bearer' [space] and paste token. 
                                    Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2OTc3MTc2MjksImV4cCI6MTY5Nzc0NjQyOSwiaWF0IjoxNjk3NzE3NjI5fQ.PN7YPayllTrWESc6mdyp3XCQ1wp3FfDLZmka6_dAJsY'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                sgo.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
                // End Security



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
