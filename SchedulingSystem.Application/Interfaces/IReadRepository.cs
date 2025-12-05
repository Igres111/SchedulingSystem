using System.Linq.Expressions;

namespace SchedulingSystem.Application.Interfaces;

public interface IReadRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
}