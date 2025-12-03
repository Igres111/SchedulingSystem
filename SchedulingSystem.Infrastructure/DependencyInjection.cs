using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchedulingSystem.Infrastructure.Persistence;

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

        services.AddDbContext<SchedulingDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}