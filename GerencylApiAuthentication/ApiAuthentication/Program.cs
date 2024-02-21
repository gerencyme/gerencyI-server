
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
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//string descriptionText = File.ReadAllText("docs/gerencyi_authentication_api.txt");
string descricao = "A API Gerencyl Auth fornece uma interface segura e eficiente para autenticar usuários em sua aplicação. Ela oferece suporte a diversos métodos de autenticação, como login com senha e tokens de acesso.\r\n\r\nFuncionalidades:\r\n\r\nLogin com senha\r\nAutenticação com token\r\nGerenciamento de tokens\r\nRedefinição de senha\r\nLogout\r\n\r\nBenefícios:\r\n\r\nSegurança: Proteja sua aplicação contra acesso não autorizado.\r\nEficiência: Autentique seus usuários de forma rápida e fácil.\r\nFlexibilidade: Suporte a diversos métodos de autenticação.\r\nEscalabilidade: Suporte a um grande número de usuários.\r\nCasos de uso:\r\n\r\nLojas online\r\nSistemas de ERP\r\nAplicativos móveis\r\nAPIs RESTful\r\nSites web\r\n\r\nA documentação completa da API Gerencyl New Order está disponível em: [Documentação Gerencyi](https://www.gerencyi.com/)\r\n\r\nSuporte:\r\n\r\nSe você tiver alguma dúvida ou precisar de ajuda para usar a API, entre em contato com o suporte da Gerencyl em: [Suporte Gerencyi](https://www.gerencyi.com/)\r\n\r\nObservações:\r\n\r\nA API Gerencyl New Order é um serviço pago.\r\nVocê precisa de uma conta Gerencyl para usar a API.\r\n\r\nOutras Apis:\r\n\r\n[Gerencyi NewOrder](https://gerencyineworder.azurewebsites.net/)\r\n\r\n[Gerencyi Gateway](https://gerencyigateway.azurewebsites.net/)\r\n";
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Gerencyl",
            Version = "v1",
            Description = descricao,
            Contact = new OpenApiContact
            {
                Name = "Contact",
                Url = new Uri("https://gerencyi.com")
            },
        });

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

builder.Services.AddLogging();

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
                      try
                      {
                          // Obtenha o subject do usuário associado ao refresh token
                          var subject = context.Principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                          // Use a lógica da classe TokenJWTBuilder para gerar um novo Access Token
                          var jwtBuilder = new TokenJWTBuilder()
                              .AddSecurityKey(JwtSecurityKey.Create(jwtSettings.SecurityKey))
                              .AddIssuer(jwtSettings.Issuer)
                              .AddAudience(jwtSettings.Audience)
                              .AddSubject(subject)
                              .WithExpiration(jwtSettings.TokenExpirationMinutes)
                              .Builder();

                          // Substitua o token original pelo novo token na resposta
                          context.Response.Headers["Authorization"] = $"Bearer {jwtBuilder.Value}";
                      }
                      catch (Exception ex)
                      {
                          // Lidar com qualquer exceção durante a geração do novo token
                          throw new Exception("Erro durante a geração do novo Access Token.");
                      }
                  }
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GerencyI v2");
    });
}
else app.MapControllers();

app.MapGet("/", () => "Hello GerencyI v1!");


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();

app.Run();
