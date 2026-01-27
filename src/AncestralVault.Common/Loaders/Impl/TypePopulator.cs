// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Constants;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Utilities;

namespace AncestralVault.Common.Loaders.Impl
{
    public class TypePopulator : ITypePopulator
    {
        public void PopulateAllTypes(AncestralVaultDbContext context)
        {
            // Event Types
            AddEventType(context, EventTypes.Birth);
            AddEventType(context, EventTypes.Death);
            AddEventType(context, EventTypes.Marriage);
            AddEventType(context, EventTypes.Residence);

            // Event Role Types
            AddEventRoleType(context, EventRoleTypes.Decedent);
            AddEventRoleType(context, EventRoleTypes.Newborn);
            AddEventRoleType(context, EventRoleTypes.Resident);
            AddEventRoleType(context, EventRoleTypes.Spouse1);
            AddEventRoleType(context, EventRoleTypes.Spouse2);

            // Place types
            AddPlaceType(context, "country");
            AddPlaceType(context, "state");
            AddPlaceType(context, "county");
            AddPlaceType(context, "township");
            AddPlaceType(context, "incorporated-place");
        }


        private static void AddEventType(AncestralVaultDbContext context, string eventTypeId)
        {
            var eventType = new EventType
            {
                EventTypeId = eventTypeId,
                Name = eventTypeId.AsTitleCase()
            };

            context.EventTypes.Add(eventType);
        }


        private static void AddEventRoleType(AncestralVaultDbContext context, string eventRoleTypeId)
        {
            var roleType = new EventRoleType
            {
                EventRoleTypeId = eventRoleTypeId,
                Name = eventRoleTypeId.AsTitleCase()
            };

            context.EventRoleTypes.Add(roleType);
        }


        private static void AddPlaceType(AncestralVaultDbContext context, string placeTypeId)
        {
            var placeType = new PlaceType
            {
                PlaceTypeId = placeTypeId,
                Name = placeTypeId.AsTitleCase()
            };

            context.PlaceTypes.Add(placeType);
        }
    }
}
