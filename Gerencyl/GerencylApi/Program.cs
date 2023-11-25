using ApiAuthentication.Token;
using AutoMapper;
using Domain.Interfaces.IGeneric;
using Domain.Interfaces.IRepositorys;
using Domain.Interfaces.IServices;
using Domain.Services;
using GerencylApi.Config;
using GerencylApi.TokenJWT;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Infrastructure.Repository.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gerencyl", Version = "v1" });

    // Configuração para autenticação com Bearer Token
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "Insira o token JWT.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
        {
            { securityScheme, new[] { "Bearer" } }
        };

    c.AddSecurityRequirement(securityRequirement);
});

//JWT
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(option =>
      {
          var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
          option.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,

              ValidIssuer = jwtSettings.Issuer,
              ValidAudience = jwtSettings.Audience,
              IssuerSigningKey = JwtSecurityKey.Create(jwtSettings.SecurityKey)
          };

          option.Events = new JwtBearerEvents
          {
              OnAuthenticationFailed = context =>
              {
                  Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                  return Task.CompletedTask;
              },
              OnTokenValidated = context =>
              {
                  var token = context.SecurityToken as JwtSecurityToken;
                  if (token != null)
                  {
                      Console.WriteLine("Token Claims: " + string.Join(", ", token.Claims.Select(c => $"{c.Type}={c.Value}")));
                      Console.WriteLine("Issuer: " + token.Issuer);
                      Console.WriteLine("Audience: " + token.Audiences.FirstOrDefault());
                  }
                  return Task.CompletedTask;
              }
          };
      });



// Adicione a leitura das configurações do appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Configure as configurações do MongoDB
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

// Registra as configurações como um serviço no DI (Dependency Injection)
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Registra o RepositoryMongoDBGeneric como serviço
builder.Services.AddSingleton(typeof(IGenericMongoDb<>), typeof(RepositoryMongoDBGeneric<>));
//builder.Services.AddSingleton(typeof(IGeneric<>), typeof(RepositoryGeneric<>));
builder.Services.AddSingleton<IRepositoryDemand, DemandRepository>();
builder.Services.AddSingleton<IDemandServices, DemandServices>();
builder.Services.AddSingleton<IRepositoryProduct, ProductRepository>();
builder.Services.AddSingleton<IProductServices, ProductServices>();


// Config Auto Mapping
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


/*builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<Company>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ContextBase>();*/


/*configuration for mysql
    var configString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContextBase>(options =>
    options.UseMySql(configString, new MySqlServerVersion(new Version(8, 0, 5))));

builder.Services.AddDefaultIdentity<Company>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ContextBase>();*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gerencyl V1");
    });

    //app.MapControllers().AllowAnonymous(); //method for disable authentication
}
else app.MapControllers();

app.MapGet("/", () => "Hello World!");

/*var devClient = " http://localhost:4200 ";
app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader().WithOrigins(devClient));*/

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();

app.Run();