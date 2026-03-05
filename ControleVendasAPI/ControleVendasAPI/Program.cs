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

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

var secrectKey = builder.Configuration.GetSection("JWT").GetValue<string>("SecretKey");

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
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        
        //Gerando a chave, usando a chave simetrica usando a secrectkey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrectKey!))
    };
});

//Repositorios
builder.Services.AddScoped<ISalesRepository, SaleRepository>();
builder.Services.AddScoped<ISweetKitRepository, SweetKitRepository>();
builder.Services.AddScoped(typeof(IRepository<>), (typeof(Repository<>)));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Serviços
builder.Services.AddScoped<IRelatorioFinanceiroDto, RelatorioFinanceiroDtoService>();
builder.Services.AddIdentity<UserToken, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();//Provedor de token padrao pra criar operaçoes relacionadas a autenticação


//Mapeamentos
builder.Services.AddAutoMapper(typeof(SalesDtoMapping));
builder.Services.AddAutoMapper(typeof(SweetKitDtoMapping));

//Autenticaçao
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Api Vendas"));


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();