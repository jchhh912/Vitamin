using Infrastructure.Presistence.Context;
using Infrastructure.Presistence.Database;
using Infrastructure.Presistence.Database.Initializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Presistence;

internal static class ServiceCollectionExtensions
{
    private static readonly Serilog.ILogger _logger = Log.ForContext(typeof(ServiceCollectionExtensions));
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        // TODO: there must be a cleaner way to do IOptions validation...
        var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        string? rootConnectionString = databaseSettings.ConnectionString;
        if (string.IsNullOrEmpty(rootConnectionString))
        {
            throw new InvalidOperationException("DB ConnectionString is not configured.");
        }

        string? dbProvider = databaseSettings.DBProvider;
        if (string.IsNullOrEmpty(dbProvider))
        {
            throw new InvalidOperationException("DB Provider is not configured.");
        }

        _logger.Information($"Current DB Provider : {dbProvider}");

        return services
            .Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))
            .AddDbContext<ApplicationDbContext>(options => options.UseDatabase(dbProvider, rootConnectionString))
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<ApplicationDbInitializer>()
            .AddTransient<ApplicationDbSeeder>();
    }
    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        return dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.SqlServer => builder.UseSqlServer(connectionString, e =>
                                  e.MigrationsAssembly("Migrators.MSSQL")),
            DbProviderKeys.MySql => builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), e =>
                                  e.MigrationsAssembly("Migrators.MySQL")
                                   .SchemaBehavior(MySqlSchemaBehavior.Ignore)),
            _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
        };
    }
}

