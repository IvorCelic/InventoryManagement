using EdunovaAPP.Extensions;
using InventoryManagementAPP.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInventoryManagementSwaggerGen();
builder.Services.AddInventoryManagementCORS();


// Adding database
builder.Services.AddDbContext<InventoryManagementContext>(o => 
    o.UseSqlServer(builder.Configuration.GetConnectionString(name: "InventoryManagementContext"))
);


// Security
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("I can't do frontend becauseThisDoesntWork and that is RealLYFruSTrAtiNG")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false
    };
});

builder.Services.AddAuthorization();
// End Security


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    // Possibility generating call of route in CMD and Powershell
    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.ConfigObject.
        AdditionalItems.Add("requestSnippetsEnabled", true);
    });
//}

app.UseHttpsRedirection();

// Security
app.UseAuthentication();
app.UseAuthorization();
// End Security

app.MapControllers();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

// Needed for production
app.UseDefaultFiles();
app.UseDeveloperExceptionPage();
app.MapFallbackToFile("index.html");

app.Run();