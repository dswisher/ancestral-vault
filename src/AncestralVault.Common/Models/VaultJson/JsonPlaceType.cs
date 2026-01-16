// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonPlaceType : IVaultJsonEntity
    {
        [JsonPropertyName("id")]
        public required string PlaceTypeId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}
