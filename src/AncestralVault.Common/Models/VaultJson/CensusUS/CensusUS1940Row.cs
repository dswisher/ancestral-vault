// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson.CensusUS
{
    public class CensusUS1940Row
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("line")]
        public int Line { get; set; }

        [JsonPropertyName("household-number")]
        public int? HouseholdNumber { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("relation")]
        public required string Relation { get; set; }

        [JsonPropertyName("sex")]
        public required string Sex { get; set; }

        [JsonPropertyName("color")]
        public required string Color { get; set; }

        [JsonPropertyName("age")]
        public required string Age { get; set; }

        [JsonPropertyName("marital-condition")]
        public required string MaritalCondition { get; set; }

        [JsonPropertyName("birth-place")]
        public required string BirthPlace { get; set; }
    }
}
