using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using Vitamin.Core;

namespace Vitamin.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var info = $"Vitamin Version {Utils.AppVersion}\n" +
                       $"Directory: {Environment.CurrentDirectory} \n" +
                       $"OS: {Utils.TryGetFullOSVersion()} \n" +
                       $"Machine Name: {Environment.MachineName} \n" +
                       $"User Name: {Environment.UserName}";
            Trace.WriteLine(info);
            Console.WriteLine(info);

            var host = CreateHostBuilder(args).Build();
            //在程序启动时创建实例
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true)
                               .ConfigureKestrel(c => c.AddServerHeader = false)
                               .UseStartup<Startup>()
                               .ConfigureLogging(logging =>
                               {
                                   logging.SetMinimumLevel(LogLevel.Trace);
                               });
                });
    }
}
