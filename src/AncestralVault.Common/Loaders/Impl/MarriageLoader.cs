// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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


        public void LoadMarriage(LoaderContext context, JsonMarriage json)
        {
            logger.LogDebug("Loading marriage '{MarriageId}'...", json.Record.Id);

            // TODO - add source/citation for marriage

            // Create personas for the bride and groom
            var groom = context.AddPersona(json.Record.Id, "groom", json.Record.Groom.Name);
            var bride = context.AddPersona(json.Record.Id, "bride", json.Record.Bride.Name);

            // Create an event record for the marriage with roles for each spouse
            var marriageEvent = context.AddEvent("marriage", json.Record.Date);

            context.AddEventRole(groom.PersonaId, "groom", marriageEvent);
            context.AddEventRole(bride.PersonaId, "bride", marriageEvent);
        }
    }
}
