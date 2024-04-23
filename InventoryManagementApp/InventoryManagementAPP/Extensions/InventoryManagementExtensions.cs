using Microsoft.OpenApi.Models;
using System.Reflection;

namespace EdunovaAPP.Extensions
{
    /// <summary>
    /// Provides extension methods for setting up the Inventory Management application,
    /// including Swagger generation and CORS configuration.
    /// </summary>
    public static class InventoryManagementExtensions
    {
        /// <summary>
        /// Configures Swagger generation for the Inventory Management API, including
        /// setup for OpenAPI documentation, security definitions, and XML comments.
        /// </summary>
        /// <param name="Services">The service collection to which SwaggerGen is added.</param>
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
                                    Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3MTMwMjEwMTAsImV4cCI6MTcxMzA0OTgxMCwiaWF0IjoxNzEzMDIxMDEwfQ.4ZMyTBQhQTfUR6E4tFAhbq4t0yVXeXRIkRtdUjSHJd8",
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

        /// <summary>
        /// Configures Cross-Origin Resource Sharing (CORS) policy for the Inventory Management API,
        /// allowing any origin, method, or header.
        /// </summary>
        /// <param name="Services">The service collection to which CORS policies are added.</param>
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
