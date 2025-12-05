namespace SchedulingSystem.Application.Dtos.Requests;

public record CreateScheduleRequest(Guid JobId, DateOnly Date);