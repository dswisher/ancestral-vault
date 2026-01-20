// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson.CensusUS
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Classes are instantiated by JSON deserialization")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties are used by JSON deserialization")]
    public class CensusUS1930 : IVaultJsonEntity
    {
        // TODO: build a property source class and use it here
        [JsonPropertyName("source")]
        public object? Source { get; set; }

        // TODO: figure out how we want to handle source and citation
        [JsonPropertyName("citation")]
        public object? Citation { get; set; }

        [JsonPropertyName("header")]
        public required CensusUSHeader Header { get; set; }

        [JsonPropertyName("rows")]
        public required List<CensusUS1930Row> Rows { get; set; } = [];
    }
}
