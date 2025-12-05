using MediatR;
using SchedulingSystem.Application.Common.Helpers;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Queries;

public class GetSchedulesQueryHandler
    : IRequestHandler<GetSchedulesQuery, PagedResponse<ScheduleResponse>>
{
    private readonly IReadRepository<Schedule> _scheduleReadRepository;

    public GetSchedulesQueryHandler(IReadRepository<Schedule> scheduleReadRepository)
    {
        _scheduleReadRepository = scheduleReadRepository;
    }

    public async Task<PagedResponse<ScheduleResponse>> Handle(
        GetSchedulesQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var normalizedPeriod = request.Period.Trim();
        var endDate = DateRangeHelper.GetEndDate(today, normalizedPeriod);

        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var (items, totalCount) = await _scheduleReadRepository.GetPagedAsync(
            s => s.Date >= today
                 && s.Date <= endDate,
            q => q.OrderBy(s => s.Date).ThenBy(s => s.JobId),
            skip: (pageNumber - 1) * pageSize,
            take: pageSize,
            cancellationToken);

        var mapped = items
            .Select(s => new ScheduleResponse(
                s.Id,
                s.JobId,
                s.UserId,
                s.Date,
                s.Status))
            .ToList();

        return new PagedResponse<ScheduleResponse>(mapped, totalCount, pageNumber, pageSize);
    }
}
