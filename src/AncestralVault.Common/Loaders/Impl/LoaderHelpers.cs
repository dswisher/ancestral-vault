// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Assistants.Persons;
using AncestralVault.Common.Models.Assistants.Persons;
using AncestralVault.Common.Models.VaultDb;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Loaders.Impl
{
    public class LoaderHelpers : ILoaderHelpers
    {
        private readonly IPersonNameParser nameParser;

        public LoaderHelpers(IPersonNameParser nameParser)
        {
            this.nameParser = nameParser;
        }


        public string BuildPersonaId(LoaderContext context, string recordId, PersonNameParseResult parsedName)
        {
            // Use the first given name as the basis for the persona ID
            var baseId = parsedName.Surname;
            if (!string.IsNullOrEmpty(parsedName.GivenNames))
            {
                baseId = parsedName.GivenNames.Split(' ').First().ToLower();
            }

            // See if this persona ID is already in use for this record. If so, we need to add a suffix to make
            // it unique.
            // TODO - need to look in the context, as the results have not yet been saved to the database
            var personaId = $"{recordId}:{baseId}";

            // Return the result
            return personaId;
        }


        public Persona AddPersona(LoaderContext context, string? recordId, string personaName, string? personaId = null)
        {
            // Parse the name. If parsing fails, this should throw an exception.
            var parsedName = nameParser.Parse(personaName);

            // Figure out the persona ID, which is based on the first given name. The thing we need
            // to contend with is that a household or event could have multiple people with the same
            // name.
            if (string.IsNullOrEmpty(personaId))
            {
                if (string.IsNullOrEmpty(recordId))
                {
                    // TODO - make this a custom exception
                    throw new Exception($"When adding persona '{personaName}', must have either a persona ID or a record ID.");
                }

                personaId = BuildPersonaId(context, recordId, parsedName);
            }

            // Create the persona and add it to the database
            var persona = new Persona
            {
                PersonaId = personaId,
                Name = personaName,
                NamePrefix = parsedName.Prefix,
                GivenNames = parsedName.GivenNames,
                Surname = parsedName.Surname,
                NameSuffix = parsedName.Suffix,
                DataFile = context.DataFile
            };

            context.DbContext.Personas.Add(persona);

            // Return the persona, so the caller can use it
            return persona;
        }


        public async Task VerifyPersonaExists(LoaderContext context, string personaId, CancellationToken stoppingToken)
        {
            var existsInDb = await context.DbContext.Personas
                .AnyAsync(p => p.PersonaId == personaId, stoppingToken);

            if (existsInDb)
            {
                return;
            }

            // Check if it exists in the change tracker (added but not yet saved)
            var existsInTracker = context.DbContext.ChangeTracker.Entries<Persona>()
                .Any(e => e.State == EntityState.Added && e.Entity.PersonaId == personaId);

            if (!existsInTracker)
            {
                throw new InvalidOperationException($"Persona '{personaId}' does not exist in the database.");
            }
        }


        public async Task VerifyCompositePersonaExists(LoaderContext context, string compositePersonaId, CancellationToken stoppingToken)
        {
            // Check if it exists in the database
            var existsInDb = await context.DbContext.CompositePersonas
                .AnyAsync(p => p.CompositePersonaId == compositePersonaId, stoppingToken);

            if (existsInDb)
            {
                return;
            }

            // Check if it exists in the change tracker (added but not yet saved)
            var existsInTracker = context.DbContext.ChangeTracker.Entries<CompositePersona>()
                .Any(e => e.State == EntityState.Added && e.Entity.CompositePersonaId == compositePersonaId);

            if (!existsInTracker)
            {
                throw new InvalidOperationException($"Composite Persona '{compositePersonaId}' does not exist in the database or pending changes.");
            }
        }
    }
}
