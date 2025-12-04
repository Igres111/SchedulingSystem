using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Interfaces;

public interface IRoleReadRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
