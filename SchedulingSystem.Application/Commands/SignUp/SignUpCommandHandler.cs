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
    private readonly IJobReadRepository _jobReadRepository;

    public SignUpCommandHandler(
        IUserReadRepository userReadRepository,
        IUserWriteRepository userWriteRepository,
        IPasswordHasher passwordHasher,
        ILogger<SignUpCommandHandler> logger,
        IRoleReadRepository roleReadRepository,
        IJobReadRepository jobReadRepository)
    {
        _userReadRepository = userReadRepository;
        _userWriteRepository = userWriteRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _roleReadRepository = roleReadRepository;
        _jobReadRepository = jobReadRepository;
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

        var job = await _jobReadRepository.GetJobByNameAsync(command.Request.JobTitle, cancellationToken);

        if (job is null)
        {
            throw new ApplicationException($"Job '{command.Request.JobTitle}' not found.");
        }

        var user = new User
        {
            Email = normalizedEmail,
            Password = passwordHash,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow,
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            JobId = job.Id
        };

        await _userWriteRepository.AddAsync(user, cancellationToken);
        await _userWriteRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("New user signed up with email {Email}", user.Email);

        return new SignUpResponse(user.Id, user.Email, user.CreatedAt);
    }
}
