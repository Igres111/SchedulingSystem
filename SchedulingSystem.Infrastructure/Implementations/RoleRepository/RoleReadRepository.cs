using Microsoft.EntityFrameworkCore;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;
using SchedulingSystem.Infrastructure.Persistence;

namespace SchedulingSystem.Infrastructure.Implementations.RoleRepository;

public class RoleReadRepository : IRoleReadRepository
{
    private readonly SchedulingDbContext _context;

    public RoleReadRepository(SchedulingDbContext context)
    {
        _context = context;
    }

    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _context.Set<Role>()
            .AsNoTracking()
            .Where(u => u.DeletedAt == null)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}
