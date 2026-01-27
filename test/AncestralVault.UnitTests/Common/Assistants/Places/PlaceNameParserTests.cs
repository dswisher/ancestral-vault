// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Assistants.Places;
using AncestralVault.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace AncestralVault.UnitTests.Common.Assistants.Places
{
    public class PlaceNameParserTests
    {
        private readonly PlaceNameParser parser;


        public PlaceNameParserTests()
        {
            var cache = new PlaceCache();

            DatabaseTestHelpers.LoadTestPlaces(cache);

            parser = new PlaceNameParser(cache);
        }


        [Theory]
        [InlineData("Iowa", "iowa")]
        public void CanParsePlaceName(string rawPlaceName, string expectedPlaceId)
        {
            // Act
            var placeId = parser.Parse(rawPlaceName);

            // Assert
            placeId.Should().Be(expectedPlaceId);
        }
    }
}
