using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Application.Dtos.Requests;

public record UpdateScheduleStatusRequest(ScheduleRequestStatus Status);