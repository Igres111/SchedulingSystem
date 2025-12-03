using SchedulingSystem.Domain.BaseTypes;
using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Domain.Entities;

public class ScheduleRequest : BaseEntity
{
    public Guid Id { get; set; }

    public Guid JobId { get; set; }
    public Job Job { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateOnly Date { get; set; }
    public ShiftType ShiftType { get; set; }

    public ScheduleRequestStatus Status { get; set; } = ScheduleRequestStatus.Pending;

    public Guid? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAtUtc { get; set; }
}