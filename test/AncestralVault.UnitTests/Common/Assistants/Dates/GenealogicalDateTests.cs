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

            output.Should().Be(expectedOutput);
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
    }
}
