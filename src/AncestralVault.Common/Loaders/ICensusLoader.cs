// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Common.Models.Loader;
using AncestralVault.Common.Models.VaultDb;

namespace AncestralVault.Common.Loaders
{
    public interface ICensusLoader
    {
        void LoadCensus(AncestralVaultDbContext dbContext, DataFile dataFile, LoaderCensus census);
    }
}
