// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Assistants.Places;
using AncestralVault.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace AncestralVault.UnitTests.Common.Assistants.Places
{
    public class PlaceNameFormatterTests
    {
        private readonly PlaceNameFormatter formatter;


        public PlaceNameFormatterTests()
        {
            var cache = new PlaceCache();
            var placeList = DatabaseTestHelpers.LoadPlacesFromEmbeddedCsv();

            cache.SeedCacheForTesting(placeList);

            formatter = new PlaceNameFormatter(cache);
        }


        [Theory]
        [InlineData("iowa", "Iowa")]
        [InlineData("mahaska", "Mahaska County, Iowa")]
        [InlineData("richland", "Richland, Mahaska County, Iowa")]
        [InlineData("new-sharon", "New Sharon, Mahaska County, Iowa")]
        public void CanFormatPlaceName(string placeId, string expectedName)
        {
            // Act
            var name = formatter.FormatName(placeId);

            // Assert
            name.Should().Be(expectedName);
        }
    }
}
