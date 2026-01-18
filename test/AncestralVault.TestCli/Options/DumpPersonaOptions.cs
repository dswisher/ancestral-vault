// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.TestCli.Options.Common;
using CommandLine;

namespace AncestralVault.TestCli.Options
{
    [Verb("dump-persona", HelpText = "Fetch a persona or composite persona from the vault and dump its details.")]
    public class DumpPersonaOptions : ILogOptions
    {
        // ILogOptions
        [Option("verbose", HelpText = "Enable verbose (debug) logging.")]
        public bool Verbose { get; set; }

        // Command-specific options
        [Option("vault", HelpText = "The directory containing the vault.")]
        public string? VaultPath { get; set; }

        [Option("persona-id", HelpText = "The ID of a persona to dump.")]
        public string? PersonaId { get; set; }

        [Option("composite-persona-id", HelpText = "The ID of a composite persona to dump.")]
        public string? CompositePersonaId { get; set; }
    }
}
