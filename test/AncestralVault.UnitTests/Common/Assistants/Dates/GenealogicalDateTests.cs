// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using AncestralVault.Common.Assistants.Dates;
using FluentAssertions;
using Xunit;

namespace AncestralVault.UnitTests.Common.Assistants.Dates
{
    public class GenealogicalDateTests
    {
        [Theory]
        [InlineData("1965", 1965, null, null, "1965")]
        [InlineData("Oct 14, 1909", 1909, 10, 14, "14-Oct-1909")]
        [InlineData("7-Apr-1930", 1930, 4, 7, "7-Apr-1930")]
        [InlineData("10/14/09", 1909, 10, 14, "14-Oct-1909")]
        [InlineData("Sep-1833", 1833, 9, null, "Sep-1833")]
        public void CanParseSimpleDates(string dateString, int expectedYear, int? expectedMonth, int? expectedDay, string expectedOutput)
        {
            // Arrange & Act
            var result = GenealogicalDate.Parse(dateString);
            var output = result?.ToString();

            // Assert
            result.Should().NotBeNull();

            result.Year.Should().Be(expectedYear);
            result.Month.Should().Be(expectedMonth);
            result.Day.Should().Be(expectedDay);

            result.Qualifier.Should().Be(DateQualifier.Exact);

            output.Should().Be(expectedOutput);
        }


        [Theory]
        [InlineData("ABT 1956", 1956, null, null, DateQualifier.About, null, "ABT 1956")]
        [InlineData("CIRCA 1956", 1956, null, null, DateQualifier.About, null, "ABT 1956")]
        [InlineData("EST 1956", 1956, null, null, DateQualifier.About, null, "ABT 1956")]
        [InlineData("BEF 1956", 1956, null, null, DateQualifier.Before, null, "BEF 1956")]
        [InlineData("AFT 1956", 1956, null, null, DateQualifier.After, null, "AFT 1956")]
        [InlineData("BET 1950 AND 1955", 1950, null, null, DateQualifier.Between, 1955, "BET 1950 AND 1955")]
        [InlineData("ABT Oct 14, 1909", 1909, 10, 14, DateQualifier.About, null, "ABT 14-Oct-1909")]
        public void CanParseQualifiedDates(string dateString, int expectedYear, int? expectedMonth, int? expectedDay, DateQualifier expectedQualifier, int? expectedToYear, string expectedOutput)
        {
            // Arrange & Act
            var result = GenealogicalDate.Parse(dateString);
            var output = result?.ToString();

            // Assert
            result.Should().NotBeNull();

            result.Year.Should().Be(expectedYear);
            result.Month.Should().Be(expectedMonth);
            result.Day.Should().Be(expectedDay);
            result.Qualifier.Should().Be(expectedQualifier);
            result.ToYear.Should().Be(expectedToYear);
            result.IsApproximate.Should().Be(expectedQualifier != DateQualifier.Exact);

            output.Should().Be(expectedOutput);
        }


        [Theory]
        [InlineData("15-Dec-2025", "5", "ABT 2020")]
        [InlineData("1-Jan-2025", "5", "ABT 2020")]
        [InlineData("15-Dec-2025", "1 2/12", "Oct-2024")]
        [InlineData("15-Dec-2025", "11/12", "Jan-2025")]
        [InlineData("1-Jan-2025", "1/12", "Dec-2024")]
        [InlineData("1-Jan-2025", "2/12", "Nov-2024")]
        public void CanSubtractAge(string startDateString, string age, string expectedDateString)
        {
            // Arrange
            var startDate = GenealogicalDate.Parse(startDateString);

            // Act
            var resultDate = startDate!.SubtractAge(age);

            // Assert
            resultDate.ToString().Should().Be(expectedDateString);
        }


        [Fact]
        public void CanSortDatesUsingOrderBy()
        {
            // Arrange
            var dates = new[]
            {
                GenealogicalDate.Parse("Oct 14, 1909"),
                GenealogicalDate.Parse("1965"),
                GenealogicalDate.Parse("7-Apr-1930"),
                GenealogicalDate.Parse("Jan 1, 1909"),
                GenealogicalDate.Parse("1909")
            };

            // Act
            var sorted = dates.OrderBy(d => d).ToList();

            // Assert
            sorted[0]!.ToString().Should().Be("1909");         // Year only comes first
            sorted[1]!.ToString().Should().Be("1-Jan-1909");   // Then Jan 1909
            sorted[2]!.ToString().Should().Be("14-Oct-1909");  // Then Oct 1909
            sorted[3]!.ToString().Should().Be("7-Apr-1930");   // Then 1930
            sorted[4]!.ToString().Should().Be("1965");         // Then 1965
        }

        [Fact]
        public void CanSortQualifiedDatesUsingOrderBy()
        {
            // Arrange
            var dates = new[]
            {
                GenealogicalDate.Parse("ABT 1956"),
                GenealogicalDate.Parse("BEF 1950"),
                GenealogicalDate.Parse("AFT 1960"),
                GenealogicalDate.Parse("BET 1940 AND 1945"),
                GenealogicalDate.Parse("1955")
            };

            // Act
            var sorted = dates.OrderBy(d => d).ToList();

            // Assert
            sorted[0]!.ToString().Should().Be("BET 1940 AND 1945");
            sorted[1]!.ToString().Should().Be("BEF 1950");
            sorted[2]!.ToString().Should().Be("1955");
            sorted[3]!.ToString().Should().Be("ABT 1956");
            sorted[4]!.ToString().Should().Be("AFT 1960");
        }
    }
}
