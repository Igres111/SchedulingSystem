using MediatR;
using Microsoft.Extensions.Logging;
using SchedulingSystem.Application.Dtos.Responses;
using SchedulingSystem.Application.Interfaces;
using SchedulingSystem.Domain.Entities;

namespace SchedulingSystem.Application.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IUserWriteRepository _userWriteRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<SignUpCommandHandler> _logger;
    private readonly IRoleReadRepository _roleReadRepository;

    public SignUpCommandHandler(
        IUserReadRepository userReadRepository,
        IUserWriteRepository userWriteRepository,
        IPasswordHasher passwordHasher,
        ILogger<SignUpCommandHandler> logger,
        IRoleReadRepository roleReadRepository)
    {
        _userReadRepository = userReadRepository;
        _userWriteRepository = userWriteRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _roleReadRepository = roleReadRepository;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        var normalizedEmail = command.Request.Email.Trim().ToLowerInvariant();

        var existingUser = await _userReadRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser is not null)
        {
            throw new ApplicationException("User with this email already exists.");
        }

        var passwordHash = _passwordHasher.Hash(command.Request.Password);

        var role = await _roleReadRepository.GetByNameAsync("User", cancellationToken);
        if (role is null)
        {
            throw new ApplicationException("Default 'User' role not found.");
        }

        var user = new User
        {
            Email = normalizedEmail,
            Password = passwordHash,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _userWriteRepository.AddAsync(user, cancellationToken);
        await _userWriteRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("New user signed up with email {Email}", user.Email);

        return new SignUpResponse
        {
            Id = user.Id,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
