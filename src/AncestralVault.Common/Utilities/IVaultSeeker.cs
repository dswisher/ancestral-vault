// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;

namespace AncestralVault.Common.Utilities
{
    public interface IVaultSeeker
    {
        DirectoryInfo VaultDir { get; }
        DirectoryInfo VaultDbDir { get; }
        FileInfo VaultDbFile { get; }

        void Configure(string? vaultPath);
    }
}
