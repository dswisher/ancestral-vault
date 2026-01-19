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
            // TODO - load marriage data
            logger.LogWarning("Loading for Marriage is not yet implemented.");
        }
    }
}
