

using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册继承ITransientService服务
    /// </summary>
    internal static IServiceCollection AddServices(this IServiceCollection services) =>
            services
                .AddServices(typeof(ITransientService), ServiceLifetime.Transient);
    /// <summary>
    /// 添加服务注入
    /// </summary>
    /// <param name="interfaceType">接口类型</param>
    /// <param name="lifetime">生命周期</param>
    /// <returns>Service</returns>
    internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t)
                            && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                })
                .Where(t => t.Service is not null
                            && interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
        {
            services.AddService(type.Service!, type.Implementation, lifetime);
        }

        return services;
    }
    /// <summary>
    /// 注册服务
    /// </summary>
    internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
       lifetime switch
       {
           ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
           ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
           ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
           _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
       };
}
