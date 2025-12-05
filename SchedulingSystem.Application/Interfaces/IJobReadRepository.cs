using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Interfaces;

public interface IJobReadRepository : IReadRepository<Job>
{
    Task<Job?> GetJobByNameAsync(string name, CancellationToken cancellationToken = default);
}