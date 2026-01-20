// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Common.Models.VaultDb;

namespace AncestralVault.Common.Loaders
{
    public class LoaderContext
    {
        public required AncestralVaultDbContext DbContext { get; init; }
        public required DataFile DataFile { get; init; }
    }
}
