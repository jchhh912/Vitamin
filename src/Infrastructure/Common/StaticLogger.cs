using Serilog;

namespace Infrastructure.Common;

public static class StaticLogger
{
    /// <summary>
    /// 日志初始化
    /// </summary>
    public static void EnsureInitialized()
    {
        if (Log.Logger is not Serilog.Core.Logger)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
