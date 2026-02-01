// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace AncestralVault.Tui
{
    public class TuiOptions
    {
        [Option("vault", HelpText = "The directory containing the vault.")]
        public string? VaultPath { get; set; }
    }
}
