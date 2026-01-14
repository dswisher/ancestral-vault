// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;
using AncestralVault.Common.Models.VaultJson.CensusUS;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders
{
    public class VaultJsonLoader : IVaultJsonLoader
    {
        private readonly ILogger<VaultJsonLoader> logger;

        public VaultJsonLoader(ILogger<VaultJsonLoader> logger)
        {
            this.logger = logger;
        }


        public void LoadEntity(AncestralVaultDbContext context, DataFile dataFile, IVaultJsonEntity entity)
        {
            if (entity is JsonPersona jsonPersona)
            {
                logger.LogDebug("Loading persona '{PersonaId}', name {PersonaName}...", jsonPersona.PersonaId, jsonPersona.Name);

                var dbPersona = new Persona
                {
                    PersonaId = jsonPersona.PersonaId,
                    Name = jsonPersona.Name,
                    Description = jsonPersona.Description,
                    DataFile = dataFile
                };

                context.Personas.Add(dbPersona);
            }
            else if (entity is CensusUS1930 census1930)
            {
                // TODO - load 1930 US Census data
                logger.LogWarning("Loading for CensusUS1930 is not yet implemented.");
            }
            else if (entity is CensusUS1940 census1940)
            {
                // TODO - load 1940 US Census data
                logger.LogWarning("Loading for CensusUS1940 is not yet implemented.");
            }
            else
            {
                // TODO - add remaining types
                throw new NotImplementedException($"Loading type '{entity.GetType().FullName}' is not implemented.");
            }
        }
    }
}
