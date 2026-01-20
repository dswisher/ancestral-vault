// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Classes are instantiated by JSON deserialization")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties are used by JSON deserialization")]
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
