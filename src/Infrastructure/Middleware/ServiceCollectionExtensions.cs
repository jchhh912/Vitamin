

using Infrastructure.Middleware.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middleware;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
    services.AddScoped<ExceptionMiddleware>();

    internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}
