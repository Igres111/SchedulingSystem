namespace SchedulingSystem.Application.Dtos.Responses;

public record SignUpResponse(Guid Id, string Email, DateTime CreatedAt);