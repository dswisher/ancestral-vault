// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using AncestralVault.Common.Assistants.Dates;
using AncestralVault.Common.Assistants.Places;
using AncestralVault.Common.Constants;
using AncestralVault.Common.Models.Loader;
using AncestralVault.Common.Models.VaultDb;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders.Impl
{
    public class CensusLoader : ICensusLoader
    {
        private readonly ILoaderHelpers loaderHelpers;
        private readonly IPlaceNameParser placeParser;
        private readonly ILogger<CensusLoader> logger;

        public CensusLoader(
            ILoaderHelpers loaderHelpers,
            IPlaceNameParser placeParser,
            ILogger<CensusLoader> logger)
        {
            this.loaderHelpers = loaderHelpers;
            this.placeParser = placeParser;
            this.logger = logger;
        }


        public void LoadCensus(LoaderContext context, LoaderCensus census)
        {
            // Build a place name from the header, and parse it
            // TODO - parse census place
            var enumerationPlaceId = GetEnumerationPlaceId(census.Header);

            // Go through all the rows and create a persona for each one, keep track of them, so we
            // can go back and add events later.
            var personas = new List<AmendedRow>();
            Persona? headPersona = null;
            foreach (var row in census.Rows)
            {
                var id = string.IsNullOrEmpty(row.Id) ? $"p{personas.Count + 1}" : row.Id;
                var personaId = $"{census.Header.Id}:{id}";

                logger.LogDebug("Loading census row for persona {PersonaId}...", personaId);

                // Create a persona and add it to the DB as well as our list of amended rows
                var persona = loaderHelpers.AddPersona(context, census.Header.Id, row.Name);

                var amendedRow = new AmendedRow
                {
                    Row = row,
                    Persona = persona
                };

                personas.Add(amendedRow);

                // Pick out the head of household
                if (row.Relation.Equals("head", StringComparison.InvariantCultureIgnoreCase))
                {
                    headPersona = persona;
                }
            }

            // Create a residence event for everyone
            var residenceEvent = context.AddEvent(EventTypes.Residence, census.Header.EnumerationDate, enumerationPlaceId);
            foreach (var amendedRow in personas)
            {
                context.AddEventRole(amendedRow.Persona.PersonaId, EventRoleTypes.Resident, residenceEvent);
            }

            // Create birth events, if we have the info to do so
            foreach (var amendedRow in personas)
            {
                TryAddBirthEvent(context, census, amendedRow);
            }

            // Do something with the parent birthplace(s)
            foreach (var amendedRow in personas)
            {
                if (!string.IsNullOrEmpty(amendedRow.Row.FatherBirthPlace))
                {
                    // TODO - what to do with father's birthplace?
                    var placeId = placeParser.Parse(amendedRow.Row.FatherBirthPlace);
                }

                if (!string.IsNullOrEmpty(amendedRow.Row.MotherBirthPlace))
                {
                    // TODO - what to do with mother's birthplace?
                    var placeId = placeParser.Parse(amendedRow.Row.MotherBirthPlace);
                }
            }
        }

        private string GetEnumerationPlaceId(LoaderCensusHeader header)
        {
            var placeParts = new List<string>();

            if (!string.IsNullOrEmpty(header.IncorporatedPlace))
            {
                placeParts.Add(header.IncorporatedPlace);
            }

            if (!string.IsNullOrEmpty(header.Township))
            {
                placeParts.Add(header.Township);
            }

            if (!string.IsNullOrEmpty(header.County))
            {
                placeParts.Add(header.County);
            }

            if (!string.IsNullOrEmpty(header.State))
            {
                placeParts.Add(header.State);
            }

            var placeName = string.Join(", ", placeParts);

            return placeParser.Parse(placeName);
        }


        private void TryAddBirthEvent(LoaderContext context, LoaderCensus census, AmendedRow amendedRow)
        {
            // Grab the birthplace
            string? birthplaceId = null;
            if (!string.IsNullOrEmpty(amendedRow.Row.BirthPlace))
            {
                birthplaceId = placeParser.Parse(amendedRow.Row.BirthPlace);
            }

            // Try to figure out the birthdate
            string? birthdate = null;

            if (!string.IsNullOrEmpty(amendedRow.Row.BirthYear) && !string.IsNullOrEmpty(amendedRow.Row.BirthMonth))
            {
                birthdate = $"{amendedRow.Row.BirthMonth}-{amendedRow.Row.BirthYear}";
            }
            else if (!string.IsNullOrEmpty(amendedRow.Row.BirthYear))
            {
                birthdate = amendedRow.Row.BirthYear;
            }
            else if (!string.IsNullOrEmpty(amendedRow.Row.Age))
            {
                var enumDate = GenealogicalDate.Parse(census.Header.EnumerationDate);
                var genBirth = enumDate!.SubtractAge(amendedRow.Row.Age);
                birthdate = genBirth.ToString();
            }

            if (birthdate != null || birthplaceId != null)
            {
                // Create the birth event
                var birthEvent = context.AddEvent(EventTypes.Birth, birthdate, birthplaceId);

                // TODO - need to set the birthplace on the event!

                // Add the event role
                context.AddEventRole(amendedRow.Persona.PersonaId, EventRoleTypes.Newborn, birthEvent);
            }
        }


        private class AmendedRow
        {
            public required LoaderCensusRow Row { get; init; }
            public required Persona Persona { get; init; }
        }
    }
}
