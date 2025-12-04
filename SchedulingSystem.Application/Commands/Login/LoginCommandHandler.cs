using MediatR;
using Microsoft.Extensions.Logging;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;

namespace SchedulingSystem.Application.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUserReadRepository userReadRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<LoginCommandHandler> logger)
    {
        _userReadRepository = userReadRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var normalizedEmail = command.Request.Email.Trim().ToLowerInvariant();

        var user = await _userReadRepository.GetByEmailWithRoleAsync(normalizedEmail, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("Login failed: User with email {Email} not found.", normalizedEmail);
            throw new ApplicationException("Invalid email.");
        }

        var passwordValid = _passwordHasher.Verify(command.Request.Password, user.Password);
        if (!passwordValid)
        {
            _logger.LogWarning("Login failed: Incorrect password for email {Email}.", normalizedEmail);
            throw new ApplicationException("Invalid password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        _logger.LogInformation("User {UserId} logged in.", user.Id);

        return new LoginResponse
        {
            Role = user.Role?.Name ?? "User",
            AccessToken = token,
        };
    }
}