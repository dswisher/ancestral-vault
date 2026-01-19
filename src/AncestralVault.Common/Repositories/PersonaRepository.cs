// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels.PersonaDetails;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        public async Task<PersonaDetailsViewModel?> GetPersonaDetailsAsync(
            AncestralVaultDbContext dbContext,
            string personaId,
            CancellationToken stoppingToken)
        {
            // Find the persona (if it exists)
            var dbPersona = await dbContext.Personas
                .Include(e => e.EventRoles).ThenInclude(t => t.EventRoleType)
                .Include(persona => persona.EventRoles).ThenInclude(eventRole => eventRole.Event).ThenInclude(@event => @event.EventType)
                .SingleOrDefaultAsync(x => x.PersonaId == personaId, stoppingToken);

            if (dbPersona == null)
            {
                return null;
            }

            // Create the base view model
            var personaDetails = new PersonaDetailsViewModel
            {
                Name = dbPersona.Name,
            };

            // Populate event box items based on persona events
            foreach (var dbEventRole in dbPersona.EventRoles)
            {
                var dbEventRoleType = dbEventRole.EventRoleType;
                var dbEvent = dbEventRole.Event;
                var dbEventType = dbEvent.EventType;

                var eventBoxItem = new PersonaDetailsEventBox
                {
                    EventTypeId = dbEventType.EventTypeId,
                    EventTypeName = dbEventType.Name,
                    EventDate = dbEvent.EventDate,
                    EventRoleTypeId = dbEventRoleType.EventRoleTypeId,
                    EventRoleTypeName = dbEventRoleType.Name,
                };

                personaDetails.EventBoxItems.Add(eventBoxItem);
            }

            // Return what we've built
            return personaDetails;
        }
    }
}
