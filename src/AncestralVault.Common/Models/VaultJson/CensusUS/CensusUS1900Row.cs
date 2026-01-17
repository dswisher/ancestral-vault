// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson.CensusUS
{
    public class CensusUS1900Row
    {
        [JsonPropertyName("line")]
        public int Line { get; set; }

        [JsonPropertyName("dwelling-number")]
        public int? DwellingNumber { get; set; }

        [JsonPropertyName("family-number")]
        public int? FamilyNumber { get; set; }

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

        [JsonPropertyName("years-married")]
        public int? YearsMarried { get; set; }

        [JsonPropertyName("mother-of-how-many-children")]
        public int? MotherOfHowManyChildren { get; set; }

        [JsonPropertyName("number-of-these-children-living")]
        public int? NumberOfTheseChildrenLiving { get; set; }

        [JsonPropertyName("birth-month")]
        public required string BirthMonth { get; set; }

        [JsonPropertyName("birth-year")]
        public required string BirthYear { get; set; }

        [JsonPropertyName("birth-place")]
        public required string BirthPlace { get; set; }

        [JsonPropertyName("father-birth-place")]
        public required string FatherBirthPlace { get; set; }

        [JsonPropertyName("mother-birth-place")]
        public required string MotherBirthPlace { get; set; }
    }
}
