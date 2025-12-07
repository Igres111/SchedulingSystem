using MediatR;
using SchedulingSystem.Application.Common.Helpers;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Queries.GetSchedules;

public class GetSchedulesQueryHandler
    : IRequestHandler<GetSchedulesQuery, PagedResponse<ScheduleResponse>>
{
    private readonly IReadRepository<Schedule> _scheduleReadRepository;
    private readonly IReadRepository<User> _userReadRepository;
    private readonly IReadRepository<Job> _jobReadRepository;

    public GetSchedulesQueryHandler(
        IReadRepository<Schedule> scheduleReadRepository,
        IReadRepository<User> userReadRepository,
        IReadRepository<Job> jobReadRepository)
    {
        _scheduleReadRepository = scheduleReadRepository;
        _userReadRepository = userReadRepository;
        _jobReadRepository = jobReadRepository;
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

        var userIds = items.Select(s => s.UserId).Distinct().ToList();
        var jobIds = items.Select(s => s.JobId).Distinct().ToList();

        var users = await _userReadRepository.GetAllAsync(u => userIds.Contains(u.Id), cancellationToken);
        var jobs = await _jobReadRepository.GetAllAsync(j => jobIds.Contains(j.Id), cancellationToken);

        var userLookup = users.ToDictionary(u => u.Id);
        var jobLookup = jobs.ToDictionary(j => j.Id);

        var mapped = items
            .Select(s =>
            {
                var user = userLookup[s.UserId];
                var job = jobLookup[s.JobId];

                return new ScheduleResponse(
                    s.Id,
                    s.JobId,
                    s.UserId,
                    user.FirstName,
                    user.LastName,
                    job.Name,
                    s.Status.ToString(),
                    s.Date,
                    s.Status);
            })
            .ToList();

        return new PagedResponse<ScheduleResponse>(mapped, totalCount, pageNumber, pageSize);
    }
}
