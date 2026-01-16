// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonPerson
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("residence")]
        public string? Residence { get; set; }

        [JsonPropertyName("occupation")]
        public string? Occupation { get; set; }

        // TODO - age-at-next-birthday vs just age?
        [JsonPropertyName("age")]
        public string? Age { get; set; }

        [JsonPropertyName("marriage-number")]
        public string? MarriageNumber { get; set; }

        [JsonPropertyName("birthplace")]
        public string? Birthplace { get; set; }

        [JsonPropertyName("mother")]
        public string? Mother { get; set; }

        [JsonPropertyName("father")]
        public string? Father { get; set; }
    }
}
