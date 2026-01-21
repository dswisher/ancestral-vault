// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
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


        public void LoadCensus(LoaderContext context, LoaderCensus census)
        {
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
                var persona = context.AddPersona(census.Header.Id, id, row.Name);

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
            var residenceEvent = context.AddEvent("residence", census.Header.EnumerationDate);
            foreach (var amendedRow in personas)
            {
                context.AddEventRole(amendedRow.Persona.PersonaId, "resident", residenceEvent);
            }

            // Go back through and create events for each persona
            // TODO
        }


        private class AmendedRow
        {
            public required LoaderCensusRow Row { get; init; }
            public required Persona Persona { get; init; }
        }
    }
}
