// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.TestCli.Options.Common;
using CommandLine;

namespace AncestralVault.TestCli.Options
{
    [Verb("rebuild", HelpText = "Rebuild the database from scratch.")]
    public class RebuildOptions : ILogOptions
    {
        // ILogOptions
        [Option("verbose", HelpText = "Enable verbose (debug) logging.")]
        public bool Verbose { get; set; }

        // Command-specific options
        [Option("vault", HelpText = "The directory containing the vault.")]
        public string? VaultPath { get; set; }

        [Option("schema-only", HelpText = "Create the schema, but do not load any data.")]
        public bool SchemaOnly { get; set; }
    }
}
