using Infrastructure.Presistence.Context;
using Infrastructure.Presistence.Database.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Presistence.Database.Initializer;


internal class ApplicationDbInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApplicationDbSeeder _dbSeeder;
    private readonly ILogger<ApplicationDbInitializer> _logger;
    public ApplicationDbInitializer(ApplicationDbContext dbContext, ApplicationDbSeeder dbSeeder, ILogger<ApplicationDbInitializer> logger)
    {
        _dbContext = dbContext;
        _dbSeeder = dbSeeder;
        _logger = logger;
    }
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
            if (_dbContext.Database.GetMigrations().Any())
            {
                //异步获取待定的迁移项目
                if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    _logger.LogInformation("Applying Migrations.");
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                }
                if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    _logger.LogInformation("Connection to Database Succeeded.");

                    await _dbSeeder.SeedDatabaseAsync();
                }
            }

    }
}
