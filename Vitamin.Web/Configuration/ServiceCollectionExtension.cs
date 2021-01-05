using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Vitamin.Core;
using Vitamin.Data.Infrastructure;

namespace Vitamin.Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataStorage(this IServiceCollection services, IConfiguration configuration)
        {     
            services.AddScoped(typeof(IRepository<>), typeof(DbContextRepository<>));
         
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
