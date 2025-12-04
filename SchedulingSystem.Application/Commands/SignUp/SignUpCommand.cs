using MediatR;
using SchedulingSystem.Application.Dtos.Requests;
using SchedulingSystem.Application.Dtos.Responses;

namespace SchedulingSystem.Application.Commands.SignUp;

public record SignUpCommand(SignUpRequest Request) : IRequest<SignUpResponse>;

