// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonMarriageItem : IVaultJsonRecord
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("license-number")]
        public string? LicenseNumber { get; set; }

        [JsonPropertyName("license-date")]
        public string? LicenseDate { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("date")]
        public string? Date { get; set; }

        // TODO - does this need to be a list?
        [JsonPropertyName("witness")]
        public string? Witness { get; set; }

        [JsonPropertyName("officiant")]
        public string? Officiant { get; set; }

        [JsonPropertyName("groom")]
        public required JsonPerson Groom { get; set; }

        [JsonPropertyName("bride")]
        public required JsonPerson Bride { get; set; }
    }
}
