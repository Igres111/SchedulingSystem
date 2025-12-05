using System;

namespace SchedulingSystem.Application.Common.Helpers;

public class DateRangeHelper
{
    private const int DaysInWeek = 7;

    public static DateOnly GetEndDate(DateOnly start, string? period)
    {
        if (string.IsNullOrWhiteSpace(period))
        {
            throw new ArgumentException("Period must be provided.", nameof(period));
        }

        var normalized = period.Trim().ToLowerInvariant();

        return normalized switch
        {
            "week" => start.AddDays(DaysInWeek - 1),
            "month" => start.AddMonths(1).AddDays(-1),
            "year" => start.AddYears(1).AddDays(-1),
            _ => throw new ArgumentException("Period must be one of: week, month, year.", nameof(period))
        };
    }
}
