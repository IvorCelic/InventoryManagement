using InventoryManagementAPP.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Needed for documentation
builder.Services.AddSwaggerGen(sgo =>
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

// Adding database
builder.Services.AddDbContext<InventoryManagementContext>(o => 
    o.UseSqlServer(builder.Configuration.GetConnectionString(name: "InventoryManagementContext"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    // Possibility generating call of route in CMD and Powershell
    app.UseSwaggerUI(options =>
    {
        options.ConfigObject.AdditionalItems.Add("requestSnippetsEnabled", true);
    });
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Needed for production
app.UseDefaultFiles();
app.UseDeveloperExceptionPage();
app.MapFallbackToFile("index.html");

app.Run();
