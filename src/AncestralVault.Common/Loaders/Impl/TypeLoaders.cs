// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Loaders.Impl
{
    public class TypeLoaders : ITypeLoaders
    {
        private readonly ILogger logger;

        public TypeLoaders(ILogger<TypeLoaders> logger)
        {
            this.logger = logger;
        }


        public void LoadEventRoleType(LoaderContext context, JsonEventRoleType json)
        {
            logger.LogDebug("Loading event role type '{EventRoleTypeId}', name {EventRoleTypeName}...", json.EventRoleTypeId, json.Name);

            var dbItem = new EventRoleType
            {
                EventRoleTypeId = json.EventRoleTypeId,
                Name = json.Name,
                DataFile = context.DataFile
            };

            context.DbContext.EventRoleTypes.Add(dbItem);
        }


        public void LoadEventType(LoaderContext context, JsonEventType json)
        {
            logger.LogDebug("Loading event type '{EventTypeId}', name {EventTypeName}...", json.EventTypeId, json.Name);

            var dbItem = new EventType
            {
                EventTypeId = json.EventTypeId,
                Name = json.Name,
                DataFile = context.DataFile
            };

            context.DbContext.EventTypes.Add(dbItem);
        }


        public void LoadPlaceType(LoaderContext context, JsonPlaceType json)
        {
            logger.LogDebug("Loading place type '{PlaceTypeId}', name {PlaceTypeName}...", json.PlaceTypeId, json.Name);

            var dbPlaceType = new PlaceType
            {
                PlaceTypeId = json.PlaceTypeId,
                Name = json.Name,
                DataFile = context.DataFile
            };

            context.DbContext.PlaceTypes.Add(dbPlaceType);
        }
    }
}
