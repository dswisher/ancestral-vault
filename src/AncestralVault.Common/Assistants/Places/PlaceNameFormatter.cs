// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using AncestralVault.Common.Constants;

namespace AncestralVault.Common.Assistants.Places
{
    public class PlaceNameFormatter : IPlaceNameFormatter
    {
        private readonly IPlaceCache cache;

        public PlaceNameFormatter(IPlaceCache cache)
        {
            this.cache = cache;
        }


        public string FormatName(string placeId)
        {
            var parts = new List<string>();

            var item = cache.GetItemById(placeId);
            string? lastPlaceType = null;
            while (item != null)
            {
                // A bit of a hack, I hate hard-coding this
                if (item.PlaceId == "usa")
                {
                    break;
                }

                if (item.PlaceTypeId == PlacePartTypes.County)
                {
                    parts.Add($"{item.Name} County");
                }
                else if (item.PlaceTypeId == PlacePartTypes.Township)
                {
                    if (lastPlaceType != PlacePartTypes.IncorporatedPlace)
                    {
                        parts.Add($"{item.Name}");
                    }
                }
                else
                {
                    parts.Add(item.Name);
                }

                lastPlaceType = item.PlaceTypeId;
                item = item.ParentPlace;
            }

            return string.Join(", ", parts);
        }
    }
}
