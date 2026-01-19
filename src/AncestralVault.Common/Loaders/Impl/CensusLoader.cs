// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Common.Models.Loader;
using AncestralVault.Common.Models.VaultDb;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders.Impl
{
    public class CensusLoader : ICensusLoader
    {
        private readonly ILogger<CensusLoader> logger;

        public CensusLoader(ILogger<CensusLoader> logger)
        {
            this.logger = logger;
        }


        public void LoadCensus(AncestralVaultDbContext dbContext, DataFile dataFile, LoaderCensus census)
        {
            // Go through all the rows and load 'em
            var num = 0;
            foreach (var row in census.Rows)
            {
                num += 1;

                var id = string.IsNullOrEmpty(row.Id) ? num.ToString() : row.Id;
                var personaId = $"{census.Header.Id}:{id}";

                logger.LogDebug("Loading census row for persona {PersonaId}...", personaId);

                // Create a persona and add it
                var persona = new Persona
                {
                    PersonaId = personaId,
                    Name = row.Name,
                    DataFile = dataFile
                };

                dbContext.Personas.Add(persona);

                // TODO - add events and whatnot for census row
            }
        }
    }
}
