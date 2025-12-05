using SchedulingSystem.Domain.BaseTypes;
using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Domain.Entities;

public class Schedule : BaseEntity
{
    public Guid Id { get; set; }

    public Guid JobId { get; set; }
    public Job Job { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateOnly Date { get; set; }
    public ScheduleRequestStatus Status { get; set; } = ScheduleRequestStatus.Pending;
}