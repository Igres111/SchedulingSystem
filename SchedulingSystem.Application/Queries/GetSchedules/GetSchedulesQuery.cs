using MediatR;
using SchedulingSystem.Application.Dtos.Responses;

namespace SchedulingSystem.Application.Queries.GetSchedules;

public record GetSchedulesQuery(
    string Period,
    int PageNumber,
    int PageSize) : IRequest<PagedResponse<ScheduleResponse>>;