using Application;
using Infrastructure;
using Infrastructure.Common;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
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
builder.Services.AddApplication();
var app = builder.Build();

await app.Services.InitializeDatabasesAsync();
app.UseInfrastructure();
app.MapEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.DocExpansion(DocExpansion.None);
    });
}
app.Run();




