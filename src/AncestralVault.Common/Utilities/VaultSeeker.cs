// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;

namespace AncestralVault.Common.Utilities
{
    public class VaultSeeker : IVaultSeeker
    {
        private DirectoryInfo? vaultDir;
        private DirectoryInfo? vaultDbDir;
        private FileInfo? vaultDbFile;


        public DirectoryInfo VaultDir => vaultDir ?? throw new InvalidOperationException("VaultSeeker has not been configured.");
        public DirectoryInfo VaultDbDir => vaultDbDir ?? throw new InvalidOperationException("VaultSeeker has not been configured.");
        public FileInfo VaultDbFile => vaultDbFile ?? throw new InvalidOperationException("VaultSeeker has not been configured.");


        public void Configure(string? vaultPath)
        {
            // TODO - go hunt for the the root of the vault
            vaultDir = new DirectoryInfo(vaultPath ?? "../../../family/adopted-vault");
            vaultDbDir = new DirectoryInfo(Path.Join(vaultDir.FullName, ".db"));
            vaultDbFile = new FileInfo(Path.Join(vaultDbDir.FullName, "vault.db"));
        }
    }
}
