// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Exceptions;
using AncestralVault.Common.Models.Assistants.Places;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Assistants.Places
{
    public class PlaceCache : IPlaceCache
    {
        private List<PlaceCacheItem>? places;


        public async Task RefreshCacheIfNeededAsync(AncestralVaultDbContext context, CancellationToken stoppingToken)
        {
            // TODO - create a table that stores the last time a place was added/modified and query it to see if we are up to date

            // Load all the places from the database and populate the cache
            var dbPlaces = await context.Places.ToListAsync(stoppingToken);

            places = dbPlaces.Select(x => new PlaceCacheItem
            {
                PlaceId = x.PlaceId,
                PlaceTypeId = x.PlaceTypeId,
                Name = x.Name,
                ParentPlaceId = x.ParentPlaceId
            }).ToList();

            // Set the parent links
            SetParentLinks();
        }


        public List<PlaceCacheItem> GetPlacesByName(string name)
        {
            if (places == null)
            {
                throw new PlaceCacheException("Place cache has not been initialized.");
            }

            return places.Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }


        public PlaceCacheItem GetItemById(string placeId)
        {
            if (places == null)
            {
                throw new PlaceCacheException("Place cache has not been initialized.");
            }

            // TODO - do we want to cache this in a dictionary for faster lookup?
            return places.Single(x => x.PlaceId == placeId);
        }


        public void SeedCacheForTesting(List<PlaceCacheItem> items)
        {
            // Copy in the places
            places = items.ToList();    // Make a copy of the list

            // Set the parent links
            SetParentLinks();
        }


        private void SetParentLinks()
        {
            // Set the parent links
            var placeDict = places!.ToDictionary(x => x.PlaceId, x => x);
            foreach (var place in places!)
            {
                if (place.ParentPlaceId != null && placeDict.TryGetValue(place.ParentPlaceId, out var parentPlace))
                {
                    place.ParentPlace = parentPlace;
                }
            }
        }
    }
}
