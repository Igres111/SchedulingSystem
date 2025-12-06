using MediatR;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;
using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Application.Commands.CreateSchedule;

public class CreateScheduleCommandHandler
: IRequestHandler<CreateScheduleCommand, ScheduleResponse>
{
    private readonly IReadRepository<Schedule> _scheduleReadRepository;
    private readonly IWriteRepository<Schedule> _scheduleWriteRepository;
    private readonly IReadRepository<User> _userReadRepository;
    private readonly IReadRepository<Job> _jobReadRepository;
    private readonly IReadRepository<Role> _roleReadRepository;

    public CreateScheduleCommandHandler(
        IReadRepository<Schedule> scheduleReadRepository,
        IWriteRepository<Schedule> scheduleWriteRepository,
        IReadRepository<User> userReadRepository,
        IReadRepository<Job> jobReadRepository,
        IReadRepository<Role> roleReadRepository)
    {
        _scheduleReadRepository = scheduleReadRepository;
        _scheduleWriteRepository = scheduleWriteRepository;
        _userReadRepository = userReadRepository;
        _jobReadRepository = jobReadRepository;
        _roleReadRepository = roleReadRepository;
    }

    public async Task<ScheduleResponse> Handle(
        CreateScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var job = await _jobReadRepository.GetByIdAsync(request.JobId, cancellationToken);
        if (job is null)
        {
            throw new InvalidOperationException("Job not found.");
        }

        var role = await _roleReadRepository.GetByIdAsync(user.RoleId, cancellationToken);
        var isAdmin = string.Equals(role?.Name, "Admin", StringComparison.OrdinalIgnoreCase);

        if (!isAdmin && user.JobId != job.Id)
        {
            throw new InvalidOperationException(
                "You can only create schedules for the job assigned to your account.");
        }

        var existing = await _scheduleReadRepository.ExistsAsync(
            s => s.Date == request.Date
                 && s.JobId == request.JobId
                 && s.UserId == request.UserId,
            cancellationToken);

        if (existing)
        {
            throw new InvalidOperationException(
                "Schedule already exists in the selected time period.");
        }

        var schedule = new Schedule
        {
            JobId = request.JobId,
            UserId = request.UserId,
            Date = request.Date,
            Status = ScheduleRequestStatus.Pending
        };

        await _scheduleWriteRepository.AddAsync(schedule, cancellationToken);
        await _scheduleWriteRepository.SaveChangesAsync(cancellationToken);

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
