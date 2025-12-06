using MediatR;
using SchedulingSystem.Application.Dtos.Responses;

namespace SchedulingSystem.Application.Queries;

public record GetUserJobQuery(Guid UserId) : IRequest<UserJobResponse>;
