// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;
using AncestralVault.Common.Models.VaultJson.CensusUS;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders.Impl
{
    public class VaultJsonLoader : IVaultJsonLoader
    {
        private readonly ICensusLoader censusLoader;
        private readonly IMarriageLoader marriageLoader;
        private readonly ITombstoneLoader tombstoneLoader;
        private readonly ITypeLoaders typeLoaders;
        private readonly ILogger logger;

        public VaultJsonLoader(
            ICensusLoader censusLoader,
            IMarriageLoader marriageLoader,
            ITombstoneLoader tombstoneLoader,
            ITypeLoaders typeLoaders,
            ILogger<VaultJsonLoader> logger)
        {
            this.censusLoader = censusLoader;
            this.marriageLoader = marriageLoader;
            this.tombstoneLoader = tombstoneLoader;
            this.typeLoaders = typeLoaders;
            this.logger = logger;
        }


        public void LoadEntities(AncestralVaultDbContext context, DataFile dataFile, List<IVaultJsonEntity> entities)
        {
            foreach (var entity in entities)
            {
                logger.LogDebug("Processing entity of type {EntityType}...", entity.GetType().Name);
                LoadEntity(context, dataFile, entity);
            }
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
                    Notes = jsonPersona.Notes,
                    DataFile = dataFile
                };

                context.Personas.Add(dbPersona);
            }
            else if (entity is JsonPlace jsonPlace)
            {
                LoadPlace(context, dataFile, jsonPlace);
            }
            else if (entity is JsonPlaceType jsonPlaceType)
            {
                typeLoaders.LoadPlaceType(context, dataFile, jsonPlaceType);
            }
            else if (entity is JsonEventRoleType jsonEventRoleType)
            {
                typeLoaders.LoadEventRoleType(context, dataFile, jsonEventRoleType);
            }
            else if (entity is JsonEventType jsonEventType)
            {
                typeLoaders.LoadEventType(context, dataFile, jsonEventType);
            }
            else if (entity is CensusUS1900 census1900)
            {
                censusLoader.LoadCensus(context, dataFile, census1900.ToLoader());
            }
            else if (entity is CensusUS1930 census1930)
            {
                censusLoader.LoadCensus(context, dataFile, census1930.ToLoader());
            }
            else if (entity is CensusUS1940 census1940)
            {
                censusLoader.LoadCensus(context, dataFile, census1940.ToLoader());
            }
            else if (entity is JsonTombstone tombstone)
            {
                tombstoneLoader.LoadTombstone(context, dataFile, tombstone);
            }
            else if (entity is JsonMarriage marriage)
            {
                marriageLoader.LoadMarriage(context, dataFile, marriage);
            }
            else if (entity is JsonCompositePersona compositePersona)
            {
                LoadCompositePersona(context, dataFile, compositePersona);
            }
            else if (entity is JsonPersonaAssertion personaAssertion)
            {
                LoadPersonaAssertion(context, dataFile, personaAssertion);
            }
            else
            {
                throw new NotImplementedException($"Loading type '{entity.GetType().FullName}' is not implemented.");
            }
        }


        private void LoadPlace(AncestralVaultDbContext context, DataFile dataFile, JsonPlace json)
        {
            logger.LogDebug("Loading place '{PlaceId}', type {PlaceTypeId}, name {PlaceName}...", json.PlaceId, json.PlaceTypeId, json.Name);

            var dbPlace = new Place
            {
                PlaceId = json.PlaceId,
                PlaceTypeId = json.PlaceTypeId,
                Name = json.Name,
                ParentPlaceId = json.Parent,
                DataFile = dataFile
            };

            context.Places.Add(dbPlace);
        }


        private void LoadCompositePersona(AncestralVaultDbContext context, DataFile dataFile, JsonCompositePersona json)
        {
            logger.LogDebug("Loading composite persona for {CompositePersonaId}...", json.Id);

            var persona = new CompositePersona
            {
                CompositePersonaId = json.Id,
                Name = json.Name,
                DataFile = dataFile
            };

            context.CompositePersonas.Add(persona);
        }


        private void LoadPersonaAssertion(AncestralVaultDbContext context, DataFile dataFile, JsonPersonaAssertion json)
        {
            logger.LogDebug("Loading persona assertion for {PersonaId} -> {CompositeId}...", json.PersonaId, json.CompositePersonaId);

            var assertion = new PersonaAssertion
            {
                CompositePersonaId = json.CompositePersonaId,
                PersonaId = json.PersonaId,
                Rationale = json.Rationale,
                DataFile = dataFile
            };

            context.PersonaAssertions.Add(assertion);
        }
    }
}
