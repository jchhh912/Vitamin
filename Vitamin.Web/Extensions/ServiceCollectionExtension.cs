using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vitamin.Configuration;
using Vitamin.Configuration.Infrastructure;
using Vitamin.Core;
using Vitamin.Data;
using Vitamin.Data.Infrastructure;
using Vitamin.Data.Model;

namespace Vitamin.Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static void AddVitaminConfiguration(this IServiceCollection services, IConfigurationSection appSettings)
        {

            services.AddOptions();
            services.AddSingleton<IVitaminConfig, VitaminConfig>();
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="ConnectionName"></param>
        public static void AddDataStorage(this IServiceCollection services, string ConnectionName)
        {
            services.AddTransient<IDbConnection>(c => new SqlConnection(ConnectionName));
            services.AddScoped(typeof(IRepository<>), typeof(DbContextRepository<>));
            services.AddDbContext<VitaminDbContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlServer(ConnectionName, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            3,
                            TimeSpan.FromSeconds(30),
                            null);
                    }));
        }
        /// <summary>
        /// 批量注入服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddVitaminServices(this IServiceCollection services)
        {
            var asm = Assembly.GetAssembly(typeof(VitaminService));
            if (asm is not null)
            {
                var types = asm.GetTypes().Where(t => t.IsClass && t.IsPublic && t.Name.EndsWith("Service"));
                foreach (var t in types)
                {
                    services.AddScoped(t, t);
                }
            } 

        }
    }
}
