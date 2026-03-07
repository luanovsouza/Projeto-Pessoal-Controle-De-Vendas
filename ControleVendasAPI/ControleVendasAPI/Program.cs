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

var secrectKey = builder.Configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT:SecretKey não configurada");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//É como se fosse, "Minha aplicação usa
    //autenicação e o tipo padrao é JWT Bearer
    options.DefaultChallengeScheme =
        JwtBearerDefaults
            .AuthenticationScheme; // Isso significa que por padrao o sisttema de autenticação, vai usar token
}).AddJwtBearer(options =>
{
    options.SaveToken = true;//Significa se o token deve ser salvo apos uma autenticaçao bem sucedida
    options.RequireHttpsMetadata = false; //Indica se é preciso HTTPS para transmitir o token OBS: Em produção deve ser true
    
    //Classe que permite configurar os parametros de validaçao do token

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //Siginifica, configurações, validar a validade do Emissor da audiencia e o tempo de vida do token
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        
        //Vai validar a assinatura de chave do emissor
        ValidateIssuerSigningKey = true,
        
        //Permite ajustar o tempo entre o servidor de autenticação e aplicaçao
        ClockSkew = TimeSpan.Zero,
        
        //Os dois esta sendo atribuido o valor de audiencia e emissor
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        
        //Gerando a chave, usando a chave simetrica usando a secrectkey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrectKey))
    };
});

builder.Services.AddIdentity<UserToken, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();//Provedor de token padrao pra criar operaçoes relacionadas a autenticação


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
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Vendas");
    });
}
else
{
    app.UseHttpsRedirection(); // Só força HTTPS em produção
}

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();