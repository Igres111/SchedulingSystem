using Microsoft.EntityFrameworkCore;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;
using SchedulingSystem.Infrastructure.Persistence;
using SchedulingSystem.Infrastructure.Repositories;

namespace SchedulingSystem.Infrastructure.Implementations.UserRepository;

public class UserReadRepository : ReadRepository<User>, IUserReadRepository
{
    public UserReadRepository(SchedulingDbContext context) : base(context)
    {
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return DbSet
            .AsNoTracking()
            .Where(u => u.DeletedAt == null)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public Task<User?> GetByEmailWithRoleAsync(string email, CancellationToken cancellationToken = default)
    {
        return DbSet
            .AsNoTracking()
            .Include(u => u.Role)
            .Where(u => u.DeletedAt == null)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}