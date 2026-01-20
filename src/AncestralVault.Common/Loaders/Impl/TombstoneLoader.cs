// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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


        public void LoadTombstone(LoaderContext context, JsonTombstone json)
        {
            logger.LogDebug("Loading tombstone '{TombstoneId}'...", json.Record.Id);

            // TODO - add source/citation for tombstone

            // Create a persona and add it
            var persona = context.AddPersona(json.Record.Id, "p1", json.Record.Name);

            // If there is a birthdate, add an event for it, with a newborn role
            if (!string.IsNullOrEmpty(json.Record.BirthDate))
            {
                var birthEvent = context.AddEvent("birth", json.Record.BirthDate);
                context.AddEventRole(persona.PersonaId, "newborn", birthEvent);
            }

            // If there is a death date, add an event for it
            if (!string.IsNullOrEmpty(json.Record.DeathDate))
            {
                var deathEvent = context.AddEvent("death", json.Record.DeathDate);
                context.AddEventRole(persona.PersonaId, "decedent", deathEvent);
            }
        }
    }
}
