using System;
using System.Collections.Generic;
using System.Linq;

namespace Ideka.NetCommon;

public static class HumanReadable
{
    private static readonly SortedList<double, Func<TimeSpan, string>> Offsets =
        new SortedList<double, Func<TimeSpan, string>>
        {
            { 0.75, _ => "under a minute"},
            { 1.5, _ => "about a minute"},
            { 45, x => $"{x.TotalMinutes:F0} minutes"},
            { 90, x => "about an hour"},
            { 1440, x => $"about {x.TotalHours:F0} hours"},
            { 2880, x => "a day"},
            { 43200, x => $"{x.TotalDays:F0} days"},
            { 86400, x => "about a month"},
            { 525600, x => $"{x.TotalDays / 30:F0} months"},
            { 1051200, x => "about a year"},
            { double.MaxValue, x => $"{x.TotalDays / 365:F0} years"}
        };

    public static string ToRelativeDateUtc(this DateTime utcInput)
    {
        var suffix = DateTime.UtcNow > utcInput ? " ago" : " from now";
        var delta = new TimeSpan(Math.Abs((DateTime.UtcNow - utcInput).Ticks));
        return Offsets.First(n => delta.TotalMinutes < n.Key).Value(delta) + suffix;
    }
}
