using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Vitamin.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //var host = CreateHostBuilder(args).Build();
            //´´½¨ÊµÀý
            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //    var logger = loggerFactory.CreateLogger<Program>();
            //}
            //host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true)
                               .ConfigureKestrel(c=>c.AddServerHeader=false)
                               .UseStartup<Startup>()
                               .ConfigureLogging(logging=> 
                               {
                                   logging.SetMinimumLevel(LogLevel.Trace);
                               });
                });
    }
}
