using Microsoft.EntityFrameworkCore;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.BaseTypes;
using SchedulingSystem.Infrastructure.Persistence;

namespace SchedulingSystem.Infrastructure.Repositories;

public class WriteRepository<TEntity> : IWriteRepository<TEntity>
  where TEntity : BaseEntity
{
    protected readonly SchedulingDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public WriteRepository(SchedulingDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.DeletedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}