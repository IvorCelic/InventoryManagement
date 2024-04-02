using EdunovaAPP.Extensions;
using InventoryManagementAPP.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInventoryManagementSwaggerGen();
builder.Services.AddInventoryManagementCORS();


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

app.UseStaticFiles();

app.UseCors("CorsPolicy");

// Needed for production
app.UseDefaultFiles();
app.UseDeveloperExceptionPage();
app.MapFallbackToFile("index.html");

app.Run();
