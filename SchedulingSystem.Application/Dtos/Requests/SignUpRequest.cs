namespace SchedulingSystem.Application.Dtos.Requests;

public record SignUpRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string JobTitle);