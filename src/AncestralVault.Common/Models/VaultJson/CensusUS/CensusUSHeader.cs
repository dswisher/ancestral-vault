// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson.CensusUS
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Classes are instantiated by JSON deserialization")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties are used by JSON deserialization")]
    public class CensusUSHeader
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("state")]
        public required string State { get; set; }

        [JsonPropertyName("county")]
        public required string County { get; set; }

        // TODO - make this nullable for places that don't have townships??
        [JsonPropertyName("township")]
        public required string Township { get; set; }

        [JsonPropertyName("incorporated-place")]
        public string? IncorporatedPlace { get; set; }

        [JsonPropertyName("enumeration-district")]
        public required string EnumerationDistrict { get; set; }

        [JsonPropertyName("supervisors-district")]
        public required string SupervisorsDistrict { get; set; }

        [JsonPropertyName("enumeration-date")]
        public required string EnumerationDate { get; set; }

        [JsonPropertyName("sheet")]
        public required string Sheet { get; set; }
    }
}
