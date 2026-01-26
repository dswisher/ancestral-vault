// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;

namespace AncestralVault.Common.Loaders
{
    public interface ITypePopulator
    {
        void PopulateAllTypes(AncestralVaultDbContext context);
    }
}
