using ControleVendasAPI.Context;
using ControleVendasAPI.DTOS.Mapping;
using ControleVendasAPI.Repositories;
using ControleVendasAPI.Repositories.Interfaces;
using ControleVendasAPI.Services;
using ControleVendasAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

//Repositorios
builder.Services.AddScoped<ISalesRepository, SaleRepository>();
builder.Services.AddScoped<ISweetKitRepository, SweetKitRepository>();
builder.Services.AddScoped(typeof(IRepository<>), (typeof(Repository<>)));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Serviços
builder.Services.AddScoped<IRelatorioFinanceiroDto, RelatorioFinanceiroDtoService>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();//Provedor de token padrao pra criar operaçoes relacionadas a autenticação


//Mapeamentos
builder.Services.AddAutoMapper(typeof(SalesDtoMapping));
builder.Services.AddAutoMapper(typeof(SweetKitDtoMapping));

//Autenticaçao
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();

app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Api Vendas"));


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();