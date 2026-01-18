// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace AncestralVault.Web.Options
{
    public class WebOptions
    {
        [Option("vault", HelpText = "The directory containing the vault.")]
        public string? VaultPath { get; set; }
    }
}
