

using Infrastructure.Middleware.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middleware;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册异常处理中间件
    /// </summary>
    internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
    services.AddScoped<ExceptionMiddleware>();

    /// <summary>
    /// 异常处理中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}
