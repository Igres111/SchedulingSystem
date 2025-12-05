namespace SchedulingSystem.Application.Dtos.Responses;

public record PagedResponse<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize);