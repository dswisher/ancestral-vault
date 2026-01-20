// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders.Impl
{
    public class MarriageLoader : IMarriageLoader
    {
        private readonly ILogger logger;

        public MarriageLoader(ILogger<MarriageLoader> logger)
        {
            this.logger = logger;
        }


        public void LoadMarriage(AncestralVaultDbContext context, DataFile dataFile, JsonMarriage json)
        {
            // Create a persona record for the groom
            var groom = new Persona
            {
                PersonaId = $"{json.Record.Id}:groom",
                Name = json.Record.Groom.Name,
                DataFile = dataFile
            };

            context.Personas.Add(groom);

            // Create a persona record for the bride
            var bridge = new Persona
            {
                PersonaId = $"{json.Record.Id}:bride",
                Name = json.Record.Bride.Name,
                DataFile = dataFile
            };

            context.Personas.Add(bridge);

            // Create an event record for the marriage with roles for each spouse
            var marriageEvent = new Event
            {
                EventTypeId = "marriage",
                EventDate = json.Record.Date,
                DataFile = dataFile
            };

            context.Events.Add(marriageEvent);

            // TODO - load marriage data
            logger.LogWarning("Loading for Marriage is not yet implemented.");
        }
    }
}
