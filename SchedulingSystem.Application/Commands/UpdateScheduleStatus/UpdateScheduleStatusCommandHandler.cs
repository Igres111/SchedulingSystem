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

    public UpdateScheduleStatusCommandHandler(
        IReadRepository<Schedule> scheduleReadRepository,
        IWriteRepository<Schedule> scheduleWriteRepository)
    {
        _scheduleReadRepository = scheduleReadRepository;
        _scheduleWriteRepository = scheduleWriteRepository;
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

        return new ScheduleResponse(schedule.Id, schedule.JobId, schedule.UserId, schedule.Date, schedule.Status);
    }
}