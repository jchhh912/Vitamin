using Application.Presistence;
using Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

// Inherited from Ardalis.Specification's RepositoryBase<T>
public class ApplicationDbRepository<T> : IRepository<T> where T : class
{
    private readonly BaseDbContext _dbContext;
    public ApplicationDbRepository(BaseDbContext dbContext) => _dbContext = dbContext;
    public Task<T> GetAsync(Expression<Func<T, bool>> condition)
    {
        return _dbContext.Set<T>().FirstOrDefaultAsync(condition);
    }

    public virtual ValueTask<T> GetAsync(object key)
    {
        return _dbContext.Set<T>().FindAsync(key);
    }

    public async Task<IReadOnlyList<T>> GetAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).AsNoTracking().ToListAsync();
    }

    public IQueryable<T> GetAsQueryable()
    {
        return _dbContext.Set<T>();
    }

    public  TResult SelectFirstOrDefault<TResult>(
        ISpecification<T> spec, Expression<Func<T, TResult>> selector)
    {
        return  ApplySpecification(spec).AsNoTracking().Select(selector).FirstOrDefault();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(object key)
    {
        var entity = await GetAsync(key);
        if (entity is not null) await DeleteAsync(entity);
    }

    public int Count(ISpecification<T> spec = null)
    {
        return null != spec ? ApplySpecification(spec).Count() : _dbContext.Set<T>().Count();
    }

    public int Count(Expression<Func<T, bool>> condition)
    {
        return _dbContext.Set<T>().Count(condition);
    }

    public bool Any(ISpecification<T> spec)
    {
        return ApplySpecification(spec).Any();
    }

    public bool Any(Expression<Func<T, bool>> condition = null)
    {
        return null != condition ? _dbContext.Set<T>().Any(condition) : _dbContext.Set<T>().Any();
    }

    public async Task<IReadOnlyList<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector)
    {
        return await _dbContext.Set<T>().AsNoTracking().Select(selector).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> SelectAsync<TResult>(
        ISpecification<T> spec, Expression<Func<T, TResult>> selector)
    {
        return await ApplySpecification(spec).AsNoTracking().Select(selector).ToListAsync();
    }

    public async Task<TResult> SelectFirstOrDefaultAsync<TResult>(
        ISpecification<T> spec, Expression<Func<T, TResult>> selector)
    {
        return await ApplySpecification(spec).AsNoTracking().Select(selector).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TResult>> SelectAsync<TGroup, TResult>(
        Expression<Func<T, TGroup>> groupExpression,
        Expression<Func<IGrouping<TGroup, T>, TResult>> selector,
        ISpecification<T> spec = null)
    {
        return null != spec ?
            await ApplySpecification(spec).AsNoTracking().GroupBy(groupExpression).Select(selector).ToListAsync() :
            await _dbContext.Set<T>().AsNoTracking().GroupBy(groupExpression).Select(selector).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public Task<int> CountAsync(ISpecification<T> spec)
    {
        return ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
    }

}
