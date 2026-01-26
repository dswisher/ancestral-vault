// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Constants;
using AncestralVault.Common.Models.VaultJson;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders.Impl
{
    public class MarriageLoader : IMarriageLoader
    {
        private readonly ILoaderHelpers loaderHelpers;
        private readonly ILogger logger;

        public MarriageLoader(
            ILoaderHelpers loaderHelpers,
            ILogger<MarriageLoader> logger)
        {
            this.loaderHelpers = loaderHelpers;
            this.logger = logger;
        }


        public void LoadMarriage(LoaderContext context, JsonMarriage json)
        {
            logger.LogDebug("Loading marriage '{MarriageId}'...", json.Record.Id);

            // TODO - add source/citation for marriage

            // Create personas for the two spouses
            var spouse1 = loaderHelpers.AddPersona(context, json.Record.Id, json.Record.Groom.Name);
            var spouse2 = loaderHelpers.AddPersona(context, json.Record.Id, json.Record.Bride.Name);

            // Create an event record for the marriage with roles for each spouse
            var marriageEvent = context.AddEvent(EventTypes.Marriage, json.Record.Date);

            context.AddEventRole(spouse1.PersonaId, EventRoleTypes.Spouse1, marriageEvent);
            context.AddEventRole(spouse2.PersonaId, EventRoleTypes.Spouse2, marriageEvent);
        }
    }
}
