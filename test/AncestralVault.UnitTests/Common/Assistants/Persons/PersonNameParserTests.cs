// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Assistants.Persons;
using FluentAssertions;
using Xunit;

namespace AncestralVault.UnitTests.Common.Assistants.Persons
{
    public class PersonNameParserTests
    {
        private readonly PersonNameParser parser;

        public PersonNameParserTests()
        {
            parser = new PersonNameParser();
        }


        [Theory]
        [InlineData("John Doe", "John", "Doe")]
        [InlineData("Jane Smith", "Jane", "Smith")]
        [InlineData("Bart J. Simpson", "Bart J.", "Simpson")]
        public void CanExtractFirstAndLastNames(string rawName, string expectedGivenNames, string expectedSurname)
        {
            // Act
            var result = parser.Parse(rawName);

            // Assert
            result.Surname.Should().Be(expectedSurname);
            result.GivenNames.Should().Be(expectedGivenNames);
        }
    }
}
