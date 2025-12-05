using FluentValidation;
using SchedulingSystem.Application.Commands.CreateSchedule;

namespace SchedulingSystem.Application.Validators;

public class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty().WithMessage("JobId is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Date)
            .Must(NotBeInThePast)
            .WithMessage("Schedule date cannot be in the past.");
    }

    private bool NotBeInThePast(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        return date >= today;
    }
}