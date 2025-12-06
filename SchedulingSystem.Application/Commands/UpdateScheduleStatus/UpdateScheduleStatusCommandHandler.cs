using MediatR;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Commands.UpdateScheduleStatus;

public class UpdateScheduleStatusCommandHandler
    : IRequestHandler<UpdateScheduleStatusCommand, ScheduleResponse>
{
    private readonly IReadRepository<Schedule> _scheduleReadRepository;
    private readonly IWriteRepository<Schedule> _scheduleWriteRepository;
    private readonly IReadRepository<User> _userReadRepository;
    private readonly IReadRepository<Job> _jobReadRepository;

    public UpdateScheduleStatusCommandHandler(
        IReadRepository<Schedule> scheduleReadRepository,
        IWriteRepository<Schedule> scheduleWriteRepository,
        IReadRepository<User> userReadRepository,
        IReadRepository<Job> jobReadRepository)
    {
        _scheduleReadRepository = scheduleReadRepository;
        _scheduleWriteRepository = scheduleWriteRepository;
        _userReadRepository = userReadRepository;
        _jobReadRepository = jobReadRepository;
    }

    public async Task<ScheduleResponse> Handle(UpdateScheduleStatusCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _scheduleReadRepository.GetByIdAsync(request.ScheduleId, cancellationToken);

        if (schedule is null)
        {
            throw new InvalidOperationException("Schedule not found.");
        }

        schedule.Status = request.Status;

        await _scheduleWriteRepository.UpdateAsync(schedule, cancellationToken);
        await _scheduleWriteRepository.SaveChangesAsync(cancellationToken);

        var user = await _userReadRepository.GetByIdAsync(schedule.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User not found for schedule.");
        }

        var job = await _jobReadRepository.GetByIdAsync(schedule.JobId, cancellationToken);
        if (job is null)
        {
            throw new InvalidOperationException("Job not found for schedule.");
        }

        return new ScheduleResponse(
            schedule.Id,
            schedule.JobId,
            schedule.UserId,
            user.FirstName,
            user.LastName,
            job.Name,
            schedule.Status.ToString(),
            schedule.Date,
            schedule.Status);
    }
}
