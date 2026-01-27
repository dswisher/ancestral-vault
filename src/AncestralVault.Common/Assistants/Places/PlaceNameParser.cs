// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using AncestralVault.Common.Exceptions;

namespace AncestralVault.Common.Assistants.Places
{
    public class PlaceNameParser : IPlaceNameParser
    {
        private readonly IPlaceCache cache;

        public PlaceNameParser(IPlaceCache cache)
        {
            this.cache = cache;
        }


        /// <summary>
        /// Take a place name as entered from a source, and determine the proper place ID.
        /// </summary>
        /// <param name="rawPlaceName">The place name as it appears in a piece of evidence.</param>
        /// <returns>The place ID that corresponds to that place.</returns>
        /// <exception cref="PlaceParserException">A PlaceException is thrown if the place ID cannot be determined.</exception>
        public string Parse(string rawPlaceName)
        {
            // Split on commas. The most specific place is first, so look up that one in the cache.
            var placeBits = rawPlaceName.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var placeIds = cache.GetPlacesByName(placeBits[0]);

            // If we found exactly one item, use it!
            if (placeIds.Count == 1)
            {
                return placeIds[0].PlaceId;
            }

            // If we found more than one item, we have ambiguity, and need to look at the other parts.
            if (placeIds.Count > 1)
            {
                // TODO - handle ambiguous places
            }

            // If we found zero items, we have no idea what this place is.
            throw new PlaceParserException($"Could not determine place ID for place name '{rawPlaceName}'.");
        }
    }
}
