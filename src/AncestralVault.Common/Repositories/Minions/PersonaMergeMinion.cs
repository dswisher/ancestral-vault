// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Constants;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.ViewModels.PersonaDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Repositories.Minions
{
    public class PersonaMergeMinion : IPersonaMergeMinion
    {
        private readonly ILogger logger;

        public PersonaMergeMinion(ILogger<PersonaMergeMinion> logger)
        {
            this.logger = logger;
        }


        public async Task MergePersonaAsync(
            AncestralVaultDbContext dbContext,
            PersonaDetailsViewModel viewModel,
            Persona persona,
            CancellationToken stoppingToken)
        {
            // Fetch all the event keys for this persona
            var eventKeys = await dbContext.EventRoles
                .Where(er => er.PersonaId == persona.PersonaId)
                .Select(er => er.EventKey)
                .ToListAsync(stoppingToken);

            // Get all event roles for those events, pulling in the related data
            var allEventRoles = await dbContext.EventRoles
                .Where(er => eventKeys.Contains(er.EventKey))
                .Include(er => er.Persona)
                .Include(er => er.EventRoleType)
                .Include(er => er.Event)
                    .ThenInclude(e => e.EventType)
                .ToListAsync(stoppingToken);

            // Get a list of all the events roles for our persona. Note that this code
            // assumes that a persona only appears once per event, which I think is a
            // safe assumption.
            var personaRoles = allEventRoles
                .Where(er => er.PersonaId == persona.PersonaId)
                .ToList();

            // Now, go through all those event roles and merge them into the view model
            foreach (var personaRole in personaRoles)
            {
                // Grab all the other event roles for this event
                var otherRoles = allEventRoles
                    .Where(er => er.EventKey == personaRole.EventKey && er.PersonaId != persona.PersonaId)
                    .ToList();

                // Based on the event role type, we may want to do different things
                var eventRoleTypeId = personaRole.EventRoleType.EventRoleTypeId;

                switch (eventRoleTypeId)
                {
                    case EventRoleTypes.Resident:
                    case EventRoleTypes.Spouse1:
                    case EventRoleTypes.Spouse2:
                        MergeRepeatableRole(viewModel, personaRole);
                        break;

                    case EventRoleTypes.Newborn:
                    case EventRoleTypes.Decedent:
                        MergeSingletonRole(viewModel, personaRole);
                        break;

                    default:
                        MergeGenericRole(viewModel, personaRole);
                        break;
                }
            }
        }


        private void MergeGenericRole(PersonaDetailsViewModel viewModel, EventRole personaRole)
        {
            logger.LogWarning("...merging generic event role {EventRoleType} for persona {PersonaName} ({PersonaId})...",
                personaRole.EventRoleType.Name, personaRole.Persona.Name, personaRole.PersonaId);

            // Create a new event box item
            CreateNewRole(viewModel, personaRole);
        }


        private void MergeRepeatableRole(PersonaDetailsViewModel viewModel, EventRole personaRole)
        {
            logger.LogInformation("...merging repeatable event role {EventRoleType} for persona {PersonaName} ({PersonaId})...",
                personaRole.EventRoleType.Name, personaRole.Persona.Name, personaRole.PersonaId);

            // Look to see if there is an event that matches this one, and if so, merge this new event with that one.
            // TODO

            // Create a new event box item
            CreateNewRole(viewModel, personaRole);
        }


        private void MergeSingletonRole(PersonaDetailsViewModel viewModel, EventRole personaRole)
        {
            logger.LogDebug("...merging singleton event role {EventRoleType} for persona {PersonaName} ({PersonaId})...",
                personaRole.EventRoleType.Name, personaRole.Persona.Name, personaRole.PersonaId);

            // Look to see if this role already exists, and if so, merge it
            var existingEvent = viewModel.EventBoxItems
                .FirstOrDefault(ebi =>
                    ebi.EventTypeId == personaRole.Event.EventTypeId);

            // Add to an existing event or create a new one
            if (existingEvent != null)
            {
                AddSource(existingEvent, personaRole);

                // TODO - update the event date, if the new source is more reliable
            }
            else
            {
                CreateNewRole(viewModel, personaRole);
            }
        }


        private static void AddSource(PersonaDetailsEventBox existingEvent, EventRole personaRole)
        {
            // TODO - figure out what we want to populate in a source
            var source = new PersonaDetailsEventBoxSource
            {
                PersonaId = personaRole.PersonaId,
                EventDate = personaRole.Event.EventDate
            };

            existingEvent.Sources.Add(source);
        }


        private static void CreateNewRole(PersonaDetailsViewModel viewModel, EventRole personaRole)
        {
            // Create a new event box item
            var eventBoxItem = new PersonaDetailsEventBox
            {
                EventTypeId = personaRole.Event.EventType.EventTypeId,
                EventTypeName = personaRole.Event.EventType.Name,
                BestEventDate = personaRole.Event.EventDate,
                EventRoleTypeId = personaRole.EventRoleType.EventRoleTypeId,
                EventRoleTypeName = personaRole.EventRoleType.Name,
            };

            AddSource(eventBoxItem, personaRole);

            viewModel.EventBoxItems.Add(eventBoxItem);
        }
    }
}
