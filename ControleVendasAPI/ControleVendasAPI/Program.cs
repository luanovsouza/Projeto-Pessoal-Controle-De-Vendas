using ControleVendasAPI.Context;
using ControleVendasAPI.DTOS.Mapping;
using ControleVendasAPI.Repositories;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<ISalesRepository, SaleRepository>();
builder.Services.AddScoped<ISweetKitRepository, SweetKitRepository>();
builder.Services.AddScoped(typeof(IRepository<>), (typeof(Repository<>)));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(SalesDtoMapping));
builder.Services.AddAutoMapper(typeof(SweetKitDtoMapping));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Api Vendas"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();