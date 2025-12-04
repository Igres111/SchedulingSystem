namespace SchedulingSystem.Application.Dtos.Responses;

public record LoginResponse
{
    public string Role { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
}