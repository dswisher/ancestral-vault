// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.VaultDb;

namespace AncestralVault.Common.Loaders.Impl
{
    public static class LoaderExtensions
    {
        public static Persona AddPersona(this LoaderContext context, string recordId, string personaSuffix, string personaName)
        {
            var persona = new Persona
            {
                PersonaId = $"{recordId}:{personaSuffix}",
                Name = personaName,
                DataFile = context.DataFile
            };

            context.DbContext.Personas.Add(persona);

            return persona;
        }


        public static Event AddEvent(this LoaderContext context, string eventType, string? eventDate)
        {
            var dbEvent = new Event
            {
                EventTypeId = eventType,
                EventDate = eventDate,
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
