// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly ILoaderHelpers loaderHelpers;
        private readonly ILogger logger;

        public VaultJsonLoader(
            ICensusLoader censusLoader,
            IMarriageLoader marriageLoader,
            ITombstoneLoader tombstoneLoader,
            ITypeLoaders typeLoaders,
            ILoaderHelpers loaderHelpers,
            ILogger<VaultJsonLoader> logger)
        {
            this.censusLoader = censusLoader;
            this.marriageLoader = marriageLoader;
            this.tombstoneLoader = tombstoneLoader;
            this.typeLoaders = typeLoaders;
            this.loaderHelpers = loaderHelpers;
            this.logger = logger;
        }


        public async Task LoadEntitiesAsync(AncestralVaultDbContext dbContext, DataFile dataFile, List<IVaultJsonEntity> entities, CancellationToken stoppingToken)
        {
            var loaderContext = new LoaderContext
            {
                DbContext = dbContext,
                DataFile = dataFile
            };

            foreach (var entity in entities)
            {
                logger.LogDebug("Processing entity of type {EntityType}...", entity.GetType().Name);
                await LoadEntityAsync(loaderContext, entity, stoppingToken);
            }
        }


        private async Task LoadEntityAsync(LoaderContext context, IVaultJsonEntity entity, CancellationToken stoppingToken)
        {
            if (entity is JsonPersona jsonPersona)
            {
                logger.LogDebug("Loading persona '{PersonaId}', name {PersonaName}...", jsonPersona.PersonaId, jsonPersona.Name);

                loaderHelpers.AddPersona(context, null, jsonPersona.Name, jsonPersona.PersonaId);
            }
            else if (entity is JsonPlace jsonPlace)
            {
                LoadPlace(context, jsonPlace);
            }
            else if (entity is JsonPlaceType jsonPlaceType)
            {
                typeLoaders.LoadPlaceType(context, jsonPlaceType);
            }
            else if (entity is JsonEventRoleType jsonEventRoleType)
            {
                typeLoaders.LoadEventRoleType(context, jsonEventRoleType);
            }
            else if (entity is JsonEventType jsonEventType)
            {
                typeLoaders.LoadEventType(context, jsonEventType);
            }
            else if (entity is CensusUS1900 census1900)
            {
                censusLoader.LoadCensus(context, census1900.ToLoader());
            }
            else if (entity is CensusUS1930 census1930)
            {
                censusLoader.LoadCensus(context, census1930.ToLoader());
            }
            else if (entity is CensusUS1940 census1940)
            {
                censusLoader.LoadCensus(context, census1940.ToLoader());
            }
            else if (entity is JsonTombstone tombstone)
            {
                tombstoneLoader.LoadTombstone(context, tombstone);
            }
            else if (entity is JsonMarriage marriage)
            {
                marriageLoader.LoadMarriage(context, marriage);
            }
            else if (entity is JsonCompositePersona compositePersona)
            {
                LoadCompositePersona(context, compositePersona);
            }
            else if (entity is JsonPersonaAssertion personaAssertion)
            {
                await LoadPersonaAssertionAsync(context, personaAssertion, stoppingToken);
            }
            else
            {
                throw new NotImplementedException($"Loading type '{entity.GetType().FullName}' is not implemented.");
            }
        }


        private void LoadPlace(LoaderContext context, JsonPlace json)
        {
            logger.LogDebug("Loading place '{PlaceId}', type {PlaceTypeId}, name {PlaceName}...", json.PlaceId, json.PlaceTypeId, json.Name);

            var dbPlace = new Place
            {
                PlaceId = json.PlaceId,
                PlaceTypeId = json.PlaceTypeId,
                Name = json.Name,
                ParentPlaceId = json.Parent,
                DataFile = context.DataFile
            };

            context.DbContext.Places.Add(dbPlace);
        }


        private void LoadCompositePersona(LoaderContext context, JsonCompositePersona json)
        {
            logger.LogDebug("Loading composite persona for {CompositePersonaId}...", json.Id);

            var persona = new CompositePersona
            {
                CompositePersonaId = json.Id,
                Name = json.Name,
                DataFile = context.DataFile
            };

            context.DbContext.CompositePersonas.Add(persona);
        }


        private async Task LoadPersonaAssertionAsync(LoaderContext context, JsonPersonaAssertion json, CancellationToken stoppingToken)
        {
            logger.LogDebug("Loading persona assertion for {PersonaId} -> {CompositeId}...", json.PersonaId, json.CompositePersonaId);

            // Verify the persona and composite persona exist
            await loaderHelpers.VerifyPersonaExists(context, json.PersonaId, stoppingToken);
            await loaderHelpers.VerifyCompositePersonaExists(context, json.CompositePersonaId, stoppingToken);

            // Create the assertion, and add it to the DB
            var assertion = new PersonaAssertion
            {
                CompositePersonaId = json.CompositePersonaId,
                PersonaId = json.PersonaId,
                Rationale = json.Rationale,
                DataFile = context.DataFile
            };

            context.DbContext.PersonaAssertions.Add(assertion);
        }
    }
}
