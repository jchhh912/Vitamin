

namespace Infrastructure.Presistence.Database.Initializer;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
}
