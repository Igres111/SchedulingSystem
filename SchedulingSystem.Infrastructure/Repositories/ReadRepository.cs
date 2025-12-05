using Microsoft.EntityFrameworkCore;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace SchedulingSystem.Infrastructure.Repositories;

public class ReadRepository<TEntity> : IReadRepository<TEntity>
    where TEntity : class
{
    protected readonly SchedulingDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public ReadRepository(SchedulingDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(id, cancellationToken);
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return DbSet.AsNoTracking().AnyAsync(expression, cancellationToken);
    }

    public async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await orderBy(query)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}