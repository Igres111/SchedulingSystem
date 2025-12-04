using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}