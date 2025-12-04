using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchedulingSystem.Application.Commands.Login;
using SchedulingSystem.Application.Commands.SignUp;
using SchedulingSystem.Application.Dtos.Requests;
using SchedulingSystem.Application.Dtos.Responses;

namespace SchedulingSystem.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/auth/signup
    ///     {
    ///         "email": "user@example.com",
    ///         "password": "P@ssword123",
    ///         "confirmPassword": "P@ssword123"
    ///     }
    /// </remarks>
    /// <response code="201">User successfully created.</response>
    /// <response code="400">Validation error or email already exists.</response>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new SignUpCommand(request), cancellationToken);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Logs in a user and returns JWT access token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/auth/login
    ///     {
    ///         "email": "user@example.com",
    ///         "password": "P@ssword123"
    ///     }
    /// </remarks>
    /// <response code="200">User successfully authenticated.</response>
    /// <response code="400">Validation error or invalid credentials.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new LoginCommand(request), cancellationToken);

        return Ok(response);
    }
}