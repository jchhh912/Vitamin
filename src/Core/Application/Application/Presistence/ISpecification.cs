

using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Presistence;

/// <summary>
/// 规约模式
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISpecification<T> : ITransientService
{
    /// <summary>
    /// 根据您的实体添加表达式的地方
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }
    /// <summary>
    ///  如果要包含外键表数据，可以使用此方法添加
    /// </summary>
    Func<IQueryable<T>, IIncludableQueryable<T, object>> Include { get; }
    /// <summary>
    /// 排序
    /// </summary>
    Expression<Func<T, object>> OrderBy { get; }
    /// <summary>
    /// 排序
    /// </summary>
    Expression<Func<T, object>> OrderByDescending { get; }
    /// <summary>
    /// 获取多少条
    /// </summary>
    int Take { get; }
    /// <summary>
    /// 多少条一页
    /// </summary>
    int Skip { get; }
    /// <summary>
    /// 是否启用分页
    /// </summary>
    bool IsPagingEnabled { get; }
}
