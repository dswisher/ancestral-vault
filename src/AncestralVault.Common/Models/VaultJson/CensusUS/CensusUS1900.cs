// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson.CensusUS
{
    public class CensusUS1900 : IVaultJsonEntity
    {
        // TODO: build a property source class and use it here
        [JsonPropertyName("source")]
        public object? Source { get; set; }

        [JsonPropertyName("header")]
        public required CensusUSHeader Header { get; set; }

        [JsonPropertyName("rows")]
        public required List<CensusUS1900Row> Rows { get; set; } = [];
    }
}
