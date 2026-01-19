// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels.PersonaDetails;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Repositories
{
    public class CompositePersonaRepository : ICompositePersonaRepository
    {
        public async Task<PersonaDetailsViewModel?> GetPersonaDetailsAsync(
            AncestralVaultDbContext dbContext,
            string compositePersonaId,
            CancellationToken stoppingToken)
        {
            // Find the persona (if it exists)
            var dbPersona = await dbContext.CompositePersonas
                .SingleOrDefaultAsync(x => x.CompositePersonaId == compositePersonaId, stoppingToken);

            if (dbPersona == null)
            {
                return null;
            }

            // Create the base view model
            var personaDetails = new PersonaDetailsViewModel
            {
                Name = dbPersona.Name,
            };

            // TODO - populate events

            // Return what we've built
            return personaDetails;
        }
    }
}
