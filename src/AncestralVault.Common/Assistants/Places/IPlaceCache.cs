// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.Assistants.Places;

namespace AncestralVault.Common.Assistants.Places
{
    public interface IPlaceCache
    {
        Task RefreshCacheIfNeededAsync(AncestralVaultDbContext context, CancellationToken stoppingToken);

        List<PlaceCacheItem> GetPlacesByName(string name);

        void SeedCacheForTesting(List<PlaceCacheItem> items);
    }
}
