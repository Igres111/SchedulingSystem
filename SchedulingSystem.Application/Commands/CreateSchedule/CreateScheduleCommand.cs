using MediatR;
using SchedulingSystem.Application.Dtos.Responses;

namespace SchedulingSystem.Application.Commands.CreateSchedule;

public record CreateScheduleCommand(Guid JobId, Guid UserId, DateOnly Date) : IRequest<ScheduleResponse>;