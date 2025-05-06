using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Products.API.Common;
using Products.API.Repository;
using Products.API.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy", x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

#endregion

#region 

builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.File("Logs/Log.txt", rollingInterval: RollingInterval.Day);
    if (context.HostingEnvironment.IsDevelopment())
    {
        config.WriteTo.Console();
    }
});
#endregion

#region Auto Mapper

builder.Services.AddAutoMapper(typeof(MappingProfile));

#endregion



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CustomPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
