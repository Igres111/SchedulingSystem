using MediatR;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Queries;

public class GetUserJobQueryHandler : IRequestHandler<GetUserJobQuery, UserJobResponse>
{
    private readonly IReadRepository<User> _userReadRepository;
    private readonly IReadRepository<Job> _jobReadRepository;

    public GetUserJobQueryHandler(
        IReadRepository<User> userReadRepository,
        IReadRepository<Job> jobReadRepository)
    {
        _userReadRepository = userReadRepository;
        _jobReadRepository = jobReadRepository;
    }

    public async Task<UserJobResponse> Handle(GetUserJobQuery request, CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var job = await _jobReadRepository.GetByIdAsync(user.JobId, cancellationToken);

        if (job is null)
        {
            throw new InvalidOperationException("Job not found.");
        }

        return new UserJobResponse(job.Id, job.Name);
    }
}
