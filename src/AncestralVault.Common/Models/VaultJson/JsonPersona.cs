// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonPersona : IVaultJsonEntity
    {
        [JsonPropertyName("id")]
        public required string PersonaId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("notes")]
        public required string Notes { get; set; }
    }
}
