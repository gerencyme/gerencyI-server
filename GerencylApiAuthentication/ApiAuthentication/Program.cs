
using ApiAuthentication.Services;
using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Token;
using AutoMapper;
using GerencylApi.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPIs.Token;
using SendGrid.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using AspNetCore.Identity.Mongo;
using Interfaces.IGeneric;
using Repository.Generic;
using MongoDB.Driver;
using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesRepositories;
using ApiAuthentication.Repository;

var builder = WebApplication.CreateBuilder(args);

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

// Configure as configurações do MongoDB
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

// Registra as configurações como um serviço no DI (Dependency Injection)
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Registra o IMongoClient e IMongoDatabase
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoDbSettings.ConnectionString));
builder.Services.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbSettings.DatabaseName));

// Registra o ContextMongoDb
builder.Services.AddScoped<ContextMongoDb>();

// Registra o RepositoryMongoDBGeneric como serviço
builder.Services.AddSingleton(typeof(IGenericMongoDb<>), typeof(RepositoryMongoDBGeneric<>));
builder.Services.AddSingleton(typeof(IAuthenticationRepository), typeof(RepositoryAuthentication));

// Registra o Identity com MongoDB
builder.Services.AddIdentity<GerencylRegister, IdentityRole2>()
    .AddMongoDbStores<GerencylRegister, IdentityRole2, Guid>(
        mongoDbSettings.ConnectionString,
        mongoDbSettings.DatabaseName)
    .AddDefaultTokenProviders();


//Config Services
builder.Services.AddScoped<IAuthenticationServicess, AuthenticationService>();
builder.Services.AddScoped<EmailConfirmationService>();


// Config Auto Mapping
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
              OnTokenValidated = async context =>
              {
                  var isRefreshTokenClaim = context.Principal.Claims.FirstOrDefault(c => c.Type == "refresh_token")?.Value;
                  if (!string.IsNullOrEmpty(isRefreshTokenClaim) && isRefreshTokenClaim.ToLower() == "true")
                  {
                      // Se é um Refresh Token, use a lógica da classe TokenJWTBuilder para gerar um novo Access Token
                      var jwtBuilder = new TokenJWTBuilder()
                          .AddSecurityKey(JwtSecurityKey.Create(jwtSettings.SecurityKey))
                          .AddIssuer(jwtSettings.Issuer)
                          .AddAudience(jwtSettings.Audience)
                          .AddSubject("novousuario@exemplo.com") // Substitua pelo sub do usuário real
                          .WithExpiration(jwtSettings.TokenExpirationMinutes)
                          .Builder();

                      // Acessa diretamente a propriedade Response para substituir o token na resposta
                      context.HttpContext.Response.Headers.Add("new-access-token", jwtBuilder.Value);
                  }

                  Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                  await Task.CompletedTask;
              },
          };

      });

//SENDGRID

builder.Services.AddSendGrid(options =>
{
    options.ApiKey = builder.Configuration
    .GetSection("SendGridEmailSettings").GetValue<string>("APIKey");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gerencyl V1");
    });
}
else app.MapControllers();

app.MapGet("/", () => "Hello GerencyI!");


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();

app.Run();
