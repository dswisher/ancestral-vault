// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;

namespace AncestralVault.Common.Loaders
{
    public interface IVaultJsonLoader
    {
        void LoadEntities(AncestralVaultDbContext context, DataFile dataFile, List<IVaultJsonEntity> entities);
        void LoadEntity(AncestralVaultDbContext context, DataFile dataFile, IVaultJsonEntity entity);
    }
}
