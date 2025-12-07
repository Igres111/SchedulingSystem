using FluentValidation;
using SchedulingSystem.Application.Queries.GetSchedules;

namespace SchedulingSystem.Application.Validators;

public class GetSchedulesQueryValidator : AbstractValidator<GetSchedulesQuery>
{
    private static readonly string[] AllowedPeriods = { "week", "month", "year" };

    public GetSchedulesQueryValidator()
    {
        RuleFor(x => x.Period)
            .NotEmpty()
            .Must(p => !string.IsNullOrWhiteSpace(p) &&
                       AllowedPeriods.Contains(p.Trim().ToLowerInvariant()))
            .WithMessage("Period must be one of: week, month, year.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}
