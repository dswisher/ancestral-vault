// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Assistants.Dates;
using AncestralVault.Common.Models.VaultDb;

namespace AncestralVault.Common.Loaders.Impl
{
    public static class LoaderExtensions
    {
        // TODO - move these methods over to LoaderHelpers

        public static Event AddEvent(this LoaderContext context, string eventType, string? eventDate, string? eventPlaceId = null)
        {
            var dbEvent = new Event
            {
                EventTypeId = eventType,
                EventDate = GenealogicalDate.Parse(eventDate),
                EventPlaceId = eventPlaceId,
                DataFile = context.DataFile
            };

            context.DbContext.Events.Add(dbEvent);

            return dbEvent;
        }


        public static EventRole AddEventRole(this LoaderContext context, string personaId, string eventRoleTypeId, Event dbEvent)
        {
            var role = new EventRole
            {
                PersonaId = personaId,
                EventRoleTypeId = eventRoleTypeId,
                Event = dbEvent,
                DataFile = context.DataFile
            };

            context.DbContext.EventRoles.Add(role);

            return role;
        }
    }
}
