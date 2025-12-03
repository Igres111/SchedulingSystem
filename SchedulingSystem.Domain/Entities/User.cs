using SchedulingSystem.Domain.BaseTypes;

namespace SchedulingSystem.Domain.Entities;

public class User : BaseEntity
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<ScheduleRequest> ScheduleRequests { get; set; } = new List<ScheduleRequest>();
}