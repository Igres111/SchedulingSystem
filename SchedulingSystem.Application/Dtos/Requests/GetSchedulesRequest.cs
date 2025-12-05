namespace SchedulingSystem.Application.Dtos.Requests;

public record GetSchedulesRequest(string Period, int PageNumber, int PageSize);