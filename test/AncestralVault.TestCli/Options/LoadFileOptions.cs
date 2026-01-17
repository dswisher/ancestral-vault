// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.TestCli.Options.Common;
using CommandLine;

namespace AncestralVault.TestCli.Options
{
    [Verb("load-file", HelpText = "Load or reload a single data file.")]
    public class LoadFileOptions : ILogOptions
    {
        // ILogOptions
        [Option("verbose", HelpText = "Enable verbose (debug) logging.")]
        public bool Verbose { get; set; }

        // Command-specific options
        [Option("data-file", Required = true, HelpText = "The filename or relative path of the data file to load (e.g., '1930-mahaska-tracy-swisher.jsonc' or 'graves/dunwoody-walter.jsonc').")]
        public required string DataFile { get; set; }

        [Option("vault", HelpText = "The directory containing the vault.")]
        public string? VaultPath { get; set; }

        [Option("validate-props", HelpText = "If a property exists in the JSON but not in the C# model, log a warning.")]
        public bool ValidateProps { get; set; }
    }
}
