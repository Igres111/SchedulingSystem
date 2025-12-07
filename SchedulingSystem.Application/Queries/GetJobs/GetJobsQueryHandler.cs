using MediatR;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Queries.GetJobs;

public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, List<UserJobResponse>>
{
    private readonly IReadRepository<User> _userReadRepository;
    private readonly IReadRepository<Job> _jobReadRepository;

    public GetJobsQueryHandler(
        IReadRepository<User> userReadRepository,
        IReadRepository<Job> jobReadRepository)
    {
        _userReadRepository = userReadRepository;
        _jobReadRepository = jobReadRepository;
    }

 public async Task<List<UserJobResponse>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _jobReadRepository.GetAllAsync(
            expression: null,
            cancellationToken: cancellationToken);

        return jobs
            .Select(j => new UserJobResponse(j.Id, j.Name))
            .ToList();
    }
}