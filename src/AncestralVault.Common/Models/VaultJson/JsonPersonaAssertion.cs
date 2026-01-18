// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonPersonaAssertion : IVaultJsonEntity
    {
        [JsonPropertyName("composite-persona")]
        public required string CompositePersonaId { get; set; }

        [JsonPropertyName("persona")]
        public required string PersonaId { get; set; }

        [JsonPropertyName("rationale")]
        public string? Rationale { get; set; }
    }
}
