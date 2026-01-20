// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonTombstoneItem : IVaultJsonRecord
    {
        /// <summary>
        /// The ID for this tombstone. Used to build identifiers for sub-entities, like the persona for the
        /// person represented by this tombstone.
        /// </summary>
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        /// <summary>
        /// The name of the person on the tombstone.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// The date of birth, as it appears on the tombstone.
        /// </summary>
        [JsonPropertyName("birth-date")]
        public string? BirthDate { get; set; }

        /// <summary>
        /// The date of death, as it appears on the tombstone.
        /// </summary>
        [JsonPropertyName("death-date")]
        public string? DeathDate { get; set; }
    }
}
