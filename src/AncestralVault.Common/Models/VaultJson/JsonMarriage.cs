// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.VaultJson
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Classes are instantiated by JSON deserialization")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties are used by JSON deserialization")]
    public class JsonMarriage : IVaultJsonEntity
    {
        // TODO: build a property source class and use it here
        [JsonPropertyName("source")]
        public object? Source { get; set; }

        // TODO - add the header

        [JsonPropertyName("record")]
        public required JsonMarriageItem Record { get; set; }
    }
}
