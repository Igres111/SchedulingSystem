using SchedulingSystem.Domain.BaseTypes;

namespace SchedulingSystem.Domain.Entities;

public class Job : BaseEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
