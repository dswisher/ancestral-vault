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
        private readonly ICensusLoader censusLoader;
        private readonly ILogger<VaultJsonLoader> logger;

        public VaultJsonLoader(
            ICensusLoader censusLoader,
            ILogger<VaultJsonLoader> logger)
        {
            this.censusLoader = censusLoader;
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
                LoadPlaceType(context, dataFile, jsonPlaceType);
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
                LoadTombstone(context, dataFile, tombstone);
            }
            else if (entity is JsonMarriage marriage)
            {
                // TODO - load marriage data
                logger.LogWarning("Loading for Marriage is not yet implemented.");
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
                // TODO - add remaining types
                throw new NotImplementedException($"Loading type '{entity.GetType().FullName}' is not implemented.");
            }
        }


        private void LoadPlaceType(AncestralVaultDbContext context, DataFile dataFile, JsonPlaceType jsonPlaceType)
        {
            logger.LogDebug("Loading place type '{PlaceTypeId}', name {PlaceTypeName}...", jsonPlaceType.PlaceTypeId, jsonPlaceType.Name);

            var dbPlaceType = new PlaceType
            {
                PlaceTypeId = jsonPlaceType.PlaceTypeId,
                Name = jsonPlaceType.Name,
                DataFile = dataFile
            };

            context.PlaceTypes.Add(dbPlaceType);
        }


        private void LoadPlace(AncestralVaultDbContext context, DataFile dataFile, JsonPlace jsonPlace)
        {
            logger.LogDebug("Loading place '{PlaceId}', type {PlaceTypeId}, name {PlaceName}...", jsonPlace.PlaceId, jsonPlace.PlaceTypeId, jsonPlace.Name);

            var dbPlace = new Place
            {
                PlaceId = jsonPlace.PlaceId,
                PlaceTypeId = jsonPlace.PlaceTypeId,
                Name = jsonPlace.Name,
                ParentPlaceId = jsonPlace.Parent,
                DataFile = dataFile
            };

            context.Places.Add(dbPlace);
        }


        private void LoadCompositePersona(AncestralVaultDbContext context, DataFile dataFile, JsonCompositePersona jsonPersona)
        {
            logger.LogDebug("Loading composite persona for {CompositeId}...", jsonPersona.Id);

            var persona = new CompositePersona
            {
                CompositePersonaId = jsonPersona.Id,
                Name = jsonPersona.Name,
                DataFile = dataFile
            };

            context.CompositePersonas.Add(persona);
        }


        private void LoadPersonaAssertion(AncestralVaultDbContext context, DataFile dataFile, JsonPersonaAssertion jsonAssertion)
        {
            logger.LogDebug("Loading persona assertion for {PersonaId} -> {CompositeId}...", jsonAssertion.PersonaId, jsonAssertion.CompositePersonaId);

            var assertion = new PersonaAssertion
            {
                CompositePersonaId = jsonAssertion.CompositePersonaId,
                PersonaId = jsonAssertion.PersonaId,
                Rationale = jsonAssertion.Rationale,
                DataFile = dataFile
            };

            context.PersonaAssertions.Add(assertion);
        }


        private void LoadTombstone(AncestralVaultDbContext context, DataFile dataFile, JsonTombstone tombstone)
        {
            logger.LogDebug("Loading tombstone '{TombstoneId}'...", tombstone.Record.Id);

            // TODO - source/citation for the persona and events

            // Create a persona and add it
            var persona = new Persona
            {
                PersonaId = $"{tombstone.Record.Id}:p1",
                Name = tombstone.Record.Name,
                DataFile = dataFile
            };

            context.Personas.Add(persona);

            // If there is a birthdate, add an event for it
            if (!string.IsNullOrEmpty(tombstone.Record.BirthDate))
            {
                var birthEvent = new SoloEvent
                {
                    EventType = "birth",
                    PrincipalPersonaId = persona.PersonaId,
                    PrincipalRole = "newborn",
                    EventDate = tombstone.Record.BirthDate,
                    DataFile = dataFile
                };

                context.SoloEvents.Add(birthEvent);
            }

            // If there is a death date, add an event for it
            if (!string.IsNullOrEmpty(tombstone.Record.DeathDate))
            {
                var birthEvent = new SoloEvent
                {
                    EventType = "death",
                    PrincipalPersonaId = persona.PersonaId,
                    PrincipalRole = "decedent",
                    EventDate = tombstone.Record.DeathDate,
                    DataFile = dataFile
                };

                context.SoloEvents.Add(birthEvent);
            }
        }
    }
}
