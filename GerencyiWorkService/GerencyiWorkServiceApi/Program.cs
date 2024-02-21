using ApiAuthentication.Token;
using AutoMapper;
using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IGeneric;
using Domain.Utils;
using GerencyINewOrderApi.Config;
using GerencyIProductApi.Config;
using GerencylApi.TokenJWT;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//string descriptionText = File.ReadAllText("docs/gerencyl_new_order_api.txt");
string descricao = "A API Gerencyl New Order � uma ferramenta poderosa que permite aos usu�rios gerenciar seus pedidos de forma eficiente e eficaz. Ela oferece uma variedade de recursos que facilitam o processo de cria��o, atualiza��o e acompanhamento de pedidos.\r\n\r\nFuncionalidades:\r\n\r\nCriar novos pedidos: A API fornece um m�todo simples para criar novos pedidos, incluindo informa��es como cliente, produtos, quantidade e valor total.\r\nAtualizar pedidos existentes: Voc� pode facilmente atualizar os detalhes de um pedido existente, como status, data de entrega ou informa��es de pagamento.\r\nObter pedidos: A API oferece v�rios m�todos para recuperar pedidos, incluindo por ID, data, status ou cliente.\r\nFiltrar pedidos: Voc� pode filtrar os pedidos por diversos crit�rios para encontrar rapidamente as informa��es que precisa.\r\nExcluir pedidos: A API permite excluir pedidos que n�o s�o mais necess�rios.\r\nBenef�cios:\r\n\r\nEfici�ncia: A API automatiza o processo de gerenciamento de pedidos, economizando tempo e recursos.\r\nPrecis�o: A API ajuda a reduzir erros humanos na entrada de dados.\r\nVisibilidade: A API fornece uma vis�o geral completa de todos os seus pedidos em um s� lugar.\r\nEscalabilidade: A API pode ser facilmente dimensionada para atender �s necessidades do seu neg�cio.\r\nCasos de uso:\r\n\r\nLojas online: A API pode ser usada para gerenciar pedidos feitos em uma loja online.\r\nSistemas de ERP: A API pode ser integrada a um sistema de ERP para centralizar o gerenciamento de pedidos.\r\nAplicativos m�veis: A API pode ser usada para criar aplicativos m�veis para gerenciar pedidos em tempo real.\r\nDocumenta��o:\r\n\r\nA documenta��o completa da API Gerencyl New Order est� dispon�vel em: [Documenta��o Gerencyi](https://www.gerencyi.com/)\r\n\r\nSuporte:\r\n\r\nSe voc� tiver alguma d�vida ou precisar de ajuda para usar a API, entre em contato com o suporte da Gerencyl em: [Suporte Gerencyi](https://www.gerencyi.com/)\r\n\r\nObserva��es:\r\n\r\nA API Gerencyl New Order � um servi�o pago.\r\nVoc� precisa de uma conta Gerencyl para usar a API.\r\n\r\nOutras Apis:\r\n\r\n[Gerencyi Autentica��o](https://gerencyiauthentication.azurewebsites.net/)\r\n\r\n[Gerencyi Gateway](https://gerencyigateway.azurewebsites.net/)";
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddLogging();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "GerencyI New Order",
            Version = "v1",
            Description = descricao,
            Contact = new OpenApiContact
            {
                Name = "Contact",
                Url = new Uri("https://gerencyi.com")
            },
            /*License = new OpenApiLicense
            {
                Name = "Example License",
                Url = new Uri("https://example.com/license")
            }*/
        });

    // Configura��o para autentica��o com Bearer Token
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
                  Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                  return Task.CompletedTask;
              }
          };
      });

builder.Services.AddHttpClient();

var serviceConfig = new DIServices();
serviceConfig.MapDependencies(builder.Services);

var repositoryConfig = new DIRepository();
repositoryConfig.RegisterDependencies(builder.Services);

// Adicione a leitura das configura��es do appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Configure as configura��es do MongoDB
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

// Registra as configura��es como um servi�o no DI (Dependency Injection)
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Registra o IMongoClient e IMongoDatabase
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoDbSettings.ConnectionString));
builder.Services.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbSettings.DatabaseName));

// Registra o ContextMongoDb
builder.Services.AddScoped<ContextNewOrderApiMongoDb>();

// Registra o RepositoryMongoDBGeneric como servi�o
builder.Services.AddSingleton(typeof(IGenericMongoDb<>), typeof(RepositoryMongoDBGeneric<>));
//builder.Services.AddSingleton(typeof(IGeneric<>), typeof(RepositoryGeneric<>));

// Config Auto Mapping
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GerencyINewOrder V1");
    });

    //app.MapControllers().AllowAnonymous(); //method for disable authentication
}
else app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();

app.Run();
