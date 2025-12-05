using Microsoft.EntityFrameworkCore;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;
using SchedulingSystem.Infrastructure.Persistence;
using SchedulingSystem.Infrastructure.Repositories;

namespace SchedulingSystem.Infrastructure.Implementations.JobRepository;

public class JobReadRepository : ReadRepository<Job>, IJobReadRepository
{
    public JobReadRepository(SchedulingDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Job?> GetJobByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Name.ToLower() == name.ToLower(), cancellationToken);
    }
}