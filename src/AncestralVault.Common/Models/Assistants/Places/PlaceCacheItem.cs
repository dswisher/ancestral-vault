// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Common.Models.Assistants.Places
{
    public class PlaceCacheItem
    {
        public required string PlaceId { get; set; }
        public required string PlaceTypeId { get; set; }
        public required string Name { get; set; }

        public string? ParentPlaceId { get; set; }
        public PlaceCacheItem? ParentPlace { get; set; }
    }
}
