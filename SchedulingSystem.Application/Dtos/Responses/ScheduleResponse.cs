using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Application.Dtos.Responses;

public record ScheduleResponse(
    Guid Id,
    Guid JobId,
    Guid UserId,
    string FirstName,
    string LastName,
    string JobName,
    string StatusName,
    DateOnly Date,
    ScheduleRequestStatus Status);