

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Presistence.Database.Initializer;

internal class DatabaseInitializer : IDatabaseInitializer
{

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;
    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Begin database initialization.");
        //创建一个服务
        using var scope = _serviceProvider.CreateScope();
        //在新建时执行初始化任务
        await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>().InitializeAsync(cancellationToken);
    }
}
