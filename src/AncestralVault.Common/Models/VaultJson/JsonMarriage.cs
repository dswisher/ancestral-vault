// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    public class JsonMarriage : IVaultJsonEntity
    {
        // TODO - add the header
        // TODO - add the source/citation

        [JsonPropertyName("record")]
        public required JsonMarriageItem Record { get; set; }
    }
}
