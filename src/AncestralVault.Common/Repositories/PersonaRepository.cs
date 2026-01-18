// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.ViewModels;
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
                .Include(p => p.SoloEvents)
                .SingleOrDefaultAsync(x => x.PersonaId == personaId, stoppingToken);

            if (dbPersona == null)
            {
                return null;
            }

            // Create the base view model
            var personaDetails = new PersonaDetailsViewModel
            {
                Name = dbPersona.Name,
                Notes = dbPersona.Notes
            };

            // Populate solo events for this persona
            foreach (var soloEvent in dbPersona.SoloEvents)
            {
                personaDetails.SoloEvents.Add(ToViewModel(soloEvent));
            }

            // TODO - fetch solo events

            // Populate joint events for this persona
            // TODO - fetch joint events

            // Return what we've built
            return personaDetails;
        }


        private static SoloEventViewModel ToViewModel(SoloEvent soloEvent)
        {
            var soloEventViewModel = new SoloEventViewModel
            {
                EventType = soloEvent.EventType,
                PrincipalPersonaId = soloEvent.PrincipalPersonaId,
                PrincipalRole = soloEvent.PrincipalRole,
                EventDate = soloEvent.EventDate,
            };

            return soloEventViewModel;
        }
    }
}
