using MediatR;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Application.Commands.UpdateScheduleStatus;

public record UpdateScheduleStatusCommand(Guid ScheduleId, ScheduleRequestStatus Status) : IRequest<ScheduleResponse>;