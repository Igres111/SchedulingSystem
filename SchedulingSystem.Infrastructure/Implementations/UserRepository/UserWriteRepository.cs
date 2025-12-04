using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;
using SchedulingSystem.Infrastructure.Persistence;
using SchedulingSystem.Infrastructure.Repositories;

namespace SchedulingSystem.Infrastructure.Implementations.UserRepository;

public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
{
    public UserWriteRepository(SchedulingDbContext context) : base(context)
    {
    }
}