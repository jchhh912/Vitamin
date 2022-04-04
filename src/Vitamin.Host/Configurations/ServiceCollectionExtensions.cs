namespace Vitamin.Host.Configurations;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// 加载配置文件
    /// </summary>
    internal static ConfigureHostBuilder AddConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            //默认文件夹
            const string configurationsDirectory = "Configurations";
            var env = context.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/authsettings.json", optional: false, reloadOnChange: true);
        });
        return host;
    }
}
