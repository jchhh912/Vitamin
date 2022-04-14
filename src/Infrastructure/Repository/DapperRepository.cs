
using Application.Presistence;
using Dapper;
using Infrastructure.Presistence.Context;
using System.Data;

namespace Infrastructure.Repository;

public class DapperRepository : IDapperRepository
{
    private readonly BaseDbContext _dbContext;
    public DapperRepository(BaseDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default) where T : class
    {
        return (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction)).AsList();
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default) where T : class
    {
        var entity=await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        if (entity == null)
            throw new ArgumentNullException(string.Empty);
        return entity;
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default) where T : class
    {
        return await _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}
