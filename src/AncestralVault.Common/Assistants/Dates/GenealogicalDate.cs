// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AncestralVault.Common.Assistants.Dates
{
    public enum DateQualifier
    {
        Exact,
        About,
        Before,
        After,
        Between
    }


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
        [
            string.Empty, "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];

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

        // Pattern: "ABT 1956" or "CIRCA 1956" (About dates)
        private static readonly Regex AboutPattern = new Regex(
            @"^(ABT|CIRCA|EST|ABOUT)\s+(.*)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Pattern: "BEF 1956" or "BEFORE 1956" (Before dates)
        private static readonly Regex BeforePattern = new Regex(
            @"^(BEF|BEFORE)\s+(.*)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Pattern: "AFT 1956" or "AFTER 1956" (After dates)
        private static readonly Regex AfterPattern = new Regex(
            @"^(AFT|AFTER)\s+(.*)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Pattern: "BET 1950 AND 1955" (Between dates)
        private static readonly Regex BetweenPattern = new Regex(
            @"^(BET|BETWEEN)\s+(\d{4})\s+(AND|&)\s+(\d{4})$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);


        public int Year { get; private init; }
        public int? Month { get; private init; }
        public int? Day { get; private init; }
        public DateQualifier Qualifier { get; private set; } = DateQualifier.Exact;
        public int? ToYear { get; private init; }

        public bool IsApproximate => Qualifier != DateQualifier.Exact;


        public static GenealogicalDate? Parse(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }

            dateString = dateString.Trim();

            // Try "BET 1950 AND 1955" pattern first (most specific)
            var match = BetweenPattern.Match(dateString);
            if (match.Success)
            {
                var fromYear = int.Parse(match.Groups[2].Value);
                var toYear = int.Parse(match.Groups[4].Value);

                return new GenealogicalDate
                {
                    Year = fromYear,
                    Qualifier = DateQualifier.Between,
                    ToYear = toYear
                };
            }

            // Try "ABT 1956" pattern
            match = AboutPattern.Match(dateString);
            if (match.Success)
            {
                var datePart = match.Groups[2].Value;
                var baseDate = ParseDatePart(datePart);
                baseDate.Qualifier = DateQualifier.About;
                return baseDate;
            }

            // Try "BEF 1956" pattern
            match = BeforePattern.Match(dateString);
            if (match.Success)
            {
                var datePart = match.Groups[2].Value;
                var baseDate = ParseDatePart(datePart);
                baseDate.Qualifier = DateQualifier.Before;
                return baseDate;
            }

            // Try "AFT 1956" pattern
            match = AfterPattern.Match(dateString);
            if (match.Success)
            {
                var datePart = match.Groups[2].Value;
                var baseDate = ParseDatePart(datePart);
                baseDate.Qualifier = DateQualifier.After;
                return baseDate;
            }

            // Try exact date patterns
            return ParseDatePart(dateString);
        }


        public override string ToString()
        {
            var baseDate = Month.HasValue && Day.HasValue
                ? $"{Day}-{MonthAbbreviations[Month.Value]}-{Year}"
                : Year.ToString();

            return Qualifier switch
            {
                DateQualifier.Exact => baseDate,
                DateQualifier.About => $"ABT {baseDate}",
                DateQualifier.Before => $"BEF {baseDate}",
                DateQualifier.After => $"AFT {baseDate}",
                DateQualifier.Between => ToYear.HasValue ? $"BET {baseDate} AND {ToYear}" : baseDate,
                _ => baseDate
            };
        }


        public int CompareTo(GenealogicalDate? other)
        {
            if (other is null)
            {
                return 1;
            }

            // For sorting purposes, approximate dates should be sorted by their base date
            // but with some consideration for their qualifier type
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


        private static GenealogicalDate ParseDatePart(string datePart)
        {
            // Try year-only pattern
            var match = YearOnlyPattern.Match(datePart);
            if (match.Success)
            {
                return new GenealogicalDate
                {
                    Year = int.Parse(match.Groups[1].Value)
                };
            }

            // Try "Oct 14, 1909" pattern
            match = MonthDayYearPattern.Match(datePart);
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
            match = DayMonthYearPattern.Match(datePart);
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
            match = NumericSlashPattern.Match(datePart);
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

            throw new FormatException($"Unable to parse date: {datePart}");
        }
    }
}
