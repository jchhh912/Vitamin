using Infrastructure.Repository;


namespace Infrastructure.Presistence.Context;

public class SqlServerDbContextRepository<T> : ApplicationDbRepository<T> where T : class
{
    public SqlServerDbContextRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
