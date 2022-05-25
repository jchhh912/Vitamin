using Application.Blog;
using Infrastructure.Auth;
using Infrastructure.Common;
using Infrastructure.Cors;
using Infrastructure.Middleware;
using Infrastructure.OpenApi;
using Infrastructure.Presistence;
using Infrastructure.Presistence.Database.Initializer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{

    /// <summary>
    /// 注册基础服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddCorsPolicy()
            .AddSwaggers()
            .AddAuth(config)
            .AddExceptionMiddleware()
            .AddHealthCheck()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddPersistence(config)
            .AddServices();
    }
    /// <summary>
    /// 注册中间件服务
    /// </summary>
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        return builder
                    .UseCorsPolicy()
                    .UseExceptionMiddleware()
                    .UseAuthentication()
                    .UseAuthorization();
    }
    /// <summary>
    /// 初始化数据库
    /// </summary>
    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }
    /// <summary>
    /// 健康检查
    /// </summary>
    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
      services.AddHealthChecks().Services;
    /// <summary>
    /// 终结点配置
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        return builder;
    }
    /// <summary>
    /// 健康检查路由地址,验证后才能访问
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/ping").RequireAuthorization();
}
