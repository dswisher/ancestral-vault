// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson.CensusUS
{
    public class CensusUS1900 : IVaultJsonEntity
    {
        [JsonPropertyName("header")]
        public required CensusUSHeader Header { get; set; }

        // TODO - add the source/citation

        [JsonPropertyName("rows")]
        public required List<CensusUS1930Row> Rows { get; set; } = [];
    }
}
