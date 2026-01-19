// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders.Impl
{
    public class TombstoneLoader : ITombstoneLoader
    {
        private readonly ILogger logger;

        public TombstoneLoader(ILogger<TombstoneLoader> logger)
        {
            this.logger = logger;
        }


        public void LoadTombstone(AncestralVaultDbContext context, DataFile dataFile, JsonTombstone json)
        {
            logger.LogDebug("Loading tombstone '{TombstoneId}'...", json.Record.Id);

            // TODO - source/citation for the persona and events

            // Create a persona and add it
            var persona = new Persona
            {
                PersonaId = $"{json.Record.Id}:p1",
                Name = json.Record.Name,
                DataFile = dataFile
            };

            context.Personas.Add(persona);

            // If there is a birthdate, add an event for it, with a newborn role
            if (!string.IsNullOrEmpty(json.Record.BirthDate))
            {
                var birthEvent = new Event
                {
                    EventTypeId = "birth",
                    EventDate = json.Record.BirthDate,
                    DataFile = dataFile
                };

                var birthRole = new EventRole
                {
                    PersonaId = persona.PersonaId,
                    EventRoleTypeId = "newborn",
                    Event = birthEvent,
                    DataFile = dataFile
                };

                context.Events.Add(birthEvent);
                context.EventRoles.Add(birthRole);
            }

            // If there is a death date, add an event for it
            if (!string.IsNullOrEmpty(json.Record.DeathDate))
            {
                var deathEvent = new Event
                {
                    EventTypeId = "death",
                    EventDate = json.Record.DeathDate,
                    DataFile = dataFile
                };

                var deathRole = new EventRole
                {
                    PersonaId = persona.PersonaId,
                    EventRoleTypeId = "decedent",
                    Event = deathEvent,
                    DataFile = dataFile
                };

                context.Events.Add(deathEvent);
                context.EventRoles.Add(deathRole);
            }
        }
    }
}
