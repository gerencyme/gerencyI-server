using AutoMapper;
using Domain.Interfaces.IGeneric;
using Domain.Interfaces.IRepositorys;
using Domain.Interfaces.IServices;
using Domain.Services;
using Entities;
using GerencylApi.Config;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Infrastructure.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<Company>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ContextBase>();

builder.Services.AddSingleton(typeof(IGeneric<>), typeof(RepositoryGeneric<>));
builder.Services.AddSingleton<IRepositoryDemand, DemandRepository>();

builder.Services.AddSingleton<IDemandServices, DemandServices>();

// Config Auto Mapping
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gerencyl V1");
    });

    app.MapControllers().AllowAnonymous(); //method for disable authentication
}
else app.MapControllers();

app.MapGet("/", () => "Hello World!");

var devClient = " http://localhost:4200 ";
app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader().WithOrigins(devClient));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();

app.Run();
