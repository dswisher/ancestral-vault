// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels.PersonaDetails;
using AncestralVault.Common.Repositories.Minions;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Repositories
{
    public class CompositePersonaRepository : ICompositePersonaRepository
    {
        private readonly IPersonaMergeMinion mergeMinion;

        public CompositePersonaRepository(IPersonaMergeMinion mergeMinion)
        {
            this.mergeMinion = mergeMinion;
        }


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

            // Get the non-negated personas associated with this composite persona
            var personas = await dbContext.PersonaAssertions
                .Where(x => x.CompositePersonaId == dbPersona.CompositePersonaId && x.Negated != true)
                .Select(x => x.Persona)
                .ToListAsync(stoppingToken);

            // Merge all those personas into the view model
            foreach (var persona in personas)
            {
                await mergeMinion.MergePersonaAsync(dbContext, personaDetails, persona, stoppingToken);
            }

            // Return what we've built
            return personaDetails;
        }
    }
}
