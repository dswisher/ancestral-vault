// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Classes are instantiated by JSON deserialization")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties are used by JSON deserialization")]
    public class JsonPlace : IVaultJsonEntity
    {
        [JsonPropertyName("id")]
        public required string PlaceId { get; set; }

        [JsonPropertyName("type")]
        public required string PlaceTypeId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("parent")]
        public string? Parent { get; set; }

        [JsonPropertyName("abbreviation")]
        public string? Abbreviation { get; set; }
    }
}
