// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using AncestralVault.Common.Assistants.Places;
using AncestralVault.Common.Models.Assistants.Places;
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

            LoadTestPlaces(cache);

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


        private static void LoadTestPlaces(IPlaceCache placeCache)
        {
            // TODO - code very similar to this exists in DatabaseTestHelpers, refactor to common location

            var placeList = new List<PlaceCacheItem>();

            const string path = "AncestralVault.UnitTests.TestHelpers.TestData.places.csv";
            using (var stream = typeof(PlaceNameParserTests).Assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream!))
            {
                var row = 0;
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    row += 1;
                    if (row == 1)
                    {
                        // Skip header line
                        continue;
                    }

                    var parts = line.Split(',');

                    var placeId = parts[0];
                    var placeTypeId = parts[1];
                    var placeName = parts[2];
                    var parentPlaceId = parts.Length > 3 ? parts[3] : null;

                    // TODO - load abbreviations, if present
                    var abbreviations = parts.Length > 4 ? parts[4].Split('|') : [];

                    var item = new PlaceCacheItem
                    {
                        PlaceId = placeId,
                        PlaceTypeId = placeTypeId,
                        Name = placeName,
                        ParentPlaceId = parentPlaceId
                    };

                    placeList.Add(item);
                }
            }

            placeCache.SeedCacheForTesting(placeList);
        }
    }
}
