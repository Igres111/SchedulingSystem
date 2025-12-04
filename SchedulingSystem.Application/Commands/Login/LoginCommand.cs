using MediatR;
using SchedulingSystem.Application.Dtos.Requests;
using SchedulingSystem.Application.Dtos.Responses;

namespace SchedulingSystem.Application.Commands.Login;

public record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>;