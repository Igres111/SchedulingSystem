using Microsoft.EntityFrameworkCore;
using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Infrastructure.Persistence;

public class SchedulingDbContext(DbContextOptions<SchedulingDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(SchedulingDbContext).Assembly);
    }
}