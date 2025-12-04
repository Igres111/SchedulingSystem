namespace SchedulingSystem.Application.Dtos.Responses;

public record SignUpResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}