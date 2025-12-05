using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Application.Dtos.Responses;

public record ScheduleResponse(
    Guid Id,
    Guid JobId,
    Guid UserId,
    DateOnly Date,
    ScheduleRequestStatus Status);