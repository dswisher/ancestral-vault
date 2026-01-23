// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AncestralVault.Common.Assistants.Dates
{
    public class GenealogicalDate : IComparable<GenealogicalDate>
    {
        private static readonly Dictionary<string, int> MonthNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Jan", 1 }, { "January", 1 },
            { "Feb", 2 }, { "February", 2 },
            { "Mar", 3 }, { "March", 3 },
            { "Apr", 4 }, { "April", 4 },
            { "May", 5 },
            { "Jun", 6 }, { "June", 6 },
            { "Jul", 7 }, { "July", 7 },
            { "Aug", 8 }, { "August", 8 },
            { "Sep", 9 }, { "September", 9 },
            { "Oct", 10 }, { "October", 10 },
            { "Nov", 11 }, { "November", 11 },
            { "Dec", 12 }, { "December", 12 }
        };

        private static readonly string[] MonthAbbreviations =
        {
            string.Empty, "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        // Pattern: Year only (e.g., "1965")
        private static readonly Regex YearOnlyPattern = new Regex(
            @"^(\d{4})$",
            RegexOptions.Compiled);

        // Pattern: "Oct 14, 1909" or "October 14, 1909"
        private static readonly Regex MonthDayYearPattern = new Regex(
            @"^([A-Za-z]+)\s+(\d{1,2}),\s*(\d{4})$",
            RegexOptions.Compiled);

        // Pattern: "7-Apr-1930" (Day-Month-Year)
        private static readonly Regex DayMonthYearPattern = new Regex(
            @"^(\d{1,2})-([A-Za-z]+)-(\d{4})$",
            RegexOptions.Compiled);

        // Pattern: "10/14/09" (MM/DD/YY)
        private static readonly Regex NumericSlashPattern = new Regex(
            @"^(\d{1,2})/(\d{1,2})/(\d{2})$",
            RegexOptions.Compiled);


        public int Year { get; private set; }
        public int? Month { get; private set; }
        public int? Day { get; private set; }


        public static GenealogicalDate? Parse(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }

            dateString = dateString.Trim();

            // Try year-only pattern
            var match = YearOnlyPattern.Match(dateString);
            if (match.Success)
            {
                return new GenealogicalDate
                {
                    Year = int.Parse(match.Groups[1].Value)
                };
            }

            // Try "Oct 14, 1909" pattern
            match = MonthDayYearPattern.Match(dateString);
            if (match.Success)
            {
                var monthName = match.Groups[1].Value;
                if (!MonthNames.TryGetValue(monthName, out var month))
                {
                    throw new FormatException($"Unknown month name: {monthName}");
                }

                return new GenealogicalDate
                {
                    Year = int.Parse(match.Groups[3].Value),
                    Month = month,
                    Day = int.Parse(match.Groups[2].Value)
                };
            }

            // Try "7-Apr-1930" pattern
            match = DayMonthYearPattern.Match(dateString);
            if (match.Success)
            {
                var monthName = match.Groups[2].Value;
                if (!MonthNames.TryGetValue(monthName, out var month))
                {
                    throw new FormatException($"Unknown month name: {monthName}");
                }

                return new GenealogicalDate
                {
                    Year = int.Parse(match.Groups[3].Value),
                    Month = month,
                    Day = int.Parse(match.Groups[1].Value)
                };
            }

            // Try "10/14/09" pattern (MM/DD/YY)
            match = NumericSlashPattern.Match(dateString);
            if (match.Success)
            {
                var year = int.Parse(match.Groups[3].Value);

                // For genealogy, assume 2-digit years are in the 1900s
                year += 1900;

                return new GenealogicalDate
                {
                    Year = year,
                    Month = int.Parse(match.Groups[1].Value),
                    Day = int.Parse(match.Groups[2].Value)
                };
            }

            throw new FormatException($"Unable to parse date: {dateString}");
        }


        public override string ToString()
        {
            if (Month.HasValue && Day.HasValue)
            {
                return $"{Day}-{MonthAbbreviations[Month.Value]}-{Year}";
            }

            return Year.ToString();
        }


        public int CompareTo(GenealogicalDate? other)
        {
            if (other is null)
            {
                return 1;
            }

            var yearComparison = Year.CompareTo(other.Year);
            if (yearComparison != 0)
            {
                return yearComparison;
            }

            var monthComparison = Nullable.Compare(Month, other.Month);
            if (monthComparison != 0)
            {
                return monthComparison;
            }

            return Nullable.Compare(Day, other.Day);
        }
    }
}
