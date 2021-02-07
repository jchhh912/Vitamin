using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vitamin.ToolKits;

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
            CreateHostBuilder(args).Build().Run();
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
