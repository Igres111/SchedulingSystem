using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Interfaces;
public interface IUserReadRepository : IReadRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailWithRoleAsync(string email, CancellationToken cancellationToken = default);
}