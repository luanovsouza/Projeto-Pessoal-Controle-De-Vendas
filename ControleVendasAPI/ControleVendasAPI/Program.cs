using System.Text;
using ControleVendasAPI.Context;
using ControleVendasAPI.DTOS.Mapping;
using ControleVendasAPI.Models;
using ControleVendasAPI.Repositories;
using ControleVendasAPI.Repositories.Interfaces;
using ControleVendasAPI.Services;
using ControleVendasAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var secrectKey = builder.Configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("SecretKey não configurada");

//Configurações de autenticação
builder.Services
    .AddIdentityCore<UserToken>()
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Configurações de autenticação JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secrectKey))
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

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
builder.Services.AddScoped<ITokenService, TokenService>();


//Mapeamentos
builder.Services.AddAutoMapper(typeof(SalesDtoMapping));
builder.Services.AddAutoMapper(typeof(SweetKitDtoMapping));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Vendas");
    });
    app.UseHttpsRedirection(); // Só força HTTPS em produção

}
else
{
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
