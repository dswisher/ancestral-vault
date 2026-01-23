// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
    }
}
