using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vitamin.Core;
using Vitamin.Data;
using Vitamin.Data.Infrastructure;
using Vitamin.Data.Model;

namespace Vitamin.Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString(Constants.DbConnectionName);

            services.AddTransient<IDbConnection>(c => new SqlConnection(connStr));
            services.AddScoped(typeof(IRepository<>), typeof(DbContextRepository<>));
            services.AddDbContext<VitaminDbContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlServer(connStr, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            3,
                            TimeSpan.FromSeconds(30),
                            null);
                    }));
        }
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
