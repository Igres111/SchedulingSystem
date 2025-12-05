using FluentValidation;
using SchedulingSystem.Application.Commands.UpdateScheduleStatus;
using SchedulingSystem.Domain.Enums;

namespace SchedulingSystem.Application.Validators;

public class UpdateScheduleStatusCommandValidator : AbstractValidator<UpdateScheduleStatusCommand>
{
    public UpdateScheduleStatusCommandValidator()
    {
        RuleFor(x => x.ScheduleId)
            .NotEmpty().WithMessage("ScheduleId is required.");

        RuleFor(x => x.Status)
            .Must(status => status is ScheduleRequestStatus.Approved or ScheduleRequestStatus.Rejected)
            .WithMessage("Status must be either Approved or Rejected.");
    }
}
