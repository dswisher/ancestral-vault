// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonTombstoneItem
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("birth-date")]
        public string? BirthDate { get; set; }

        [JsonPropertyName("death-date")]
        public string? DeathDate { get; set; }
    }
}
