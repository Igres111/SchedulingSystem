using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Infrastructure.Implementations.JobRepository;
using SchedulingSystem.Infrastructure.Implementations.RoleRepository;
using SchedulingSystem.Infrastructure.Implementations.UserRepository;
using SchedulingSystem.Infrastructure.Persistence;
using SchedulingSystem.Infrastructure.Repositories;
using SchedulingSystem.Infrastructure.Security;

namespace SchedulingSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("POSTGRES_CONNECTION environment variable is not set.");

        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IRoleReadRepository, RoleReadRepository>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IJobReadRepository, JobReadRepository>();

        services.AddDbContext<SchedulingDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}