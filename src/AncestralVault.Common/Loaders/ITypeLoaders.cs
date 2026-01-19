// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;

namespace AncestralVault.Common.Loaders
{
    public interface ITypeLoaders
    {
        void LoadEventRoleType(AncestralVaultDbContext context, DataFile dataFile, JsonEventRoleType json);
        void LoadEventType(AncestralVaultDbContext context, DataFile dataFile, JsonEventType json);
        void LoadPlaceType(AncestralVaultDbContext context, DataFile dataFile, JsonPlaceType json);
    }
}
