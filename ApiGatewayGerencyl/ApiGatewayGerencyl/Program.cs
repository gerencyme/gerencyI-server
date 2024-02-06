using ApiGatewayGerencyl.TokenValidationMiddleware;
using ApiGerencyiGateway.TokenSettigns;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITokenCache, InMemoryTokenCache>();
//builder.Services.AddTransient<TokenValidationMiddleware>();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddTransient<JwtSettings>();
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenValidationMiddleware>(); // Adicione o middleware diretamente ao pipeline

app.MapGet("/", () => "Hello World!");

app.UseOcelot().Wait(); // Use o método Wait para aguardar a conclusão da execução do Ocelot
app.Run();
