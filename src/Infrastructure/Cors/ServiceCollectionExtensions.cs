
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cors;

internal static class ServiceCollectionExtensions
{
    private const string CorsPolicy=nameof(CorsPolicy);
    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services) 
    {
        return services.AddCors(opt =>
            opt.AddPolicy(CorsPolicy, policy =>
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithOrigins("http://localhost:3000")));
    }
    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)=>
        app.UseCors(CorsPolicy);
}
