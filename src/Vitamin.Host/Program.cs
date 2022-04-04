using Infrastructure;
using Infrastructure.Common;
using Serilog;
using Vitamin.Host.Configurations;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddConfigurations();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Host.UseSerilog((_, config) => 
{
    config.WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration);
});
builder.Services.AddControllers();

var app = builder.Build();
await app.Services.InitializeDatabasesAsync();
app.UseInfrastructure();
app.MapEndpoints();
app.Run();




