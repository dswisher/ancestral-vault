// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;

namespace AncestralVault.Common.Loaders
{
    public interface IVaultJsonLoader
    {
        Task LoadEntitiesAsync(AncestralVaultDbContext dbContext, DataFile dataFile, List<IVaultJsonEntity> entities, CancellationToken stoppingToken);
    }
}
