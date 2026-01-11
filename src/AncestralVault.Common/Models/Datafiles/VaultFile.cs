using System.Collections.Generic;
using System.Text.Json.Serialization;
using AncestralVault.Common.Models.Database;

namespace AncestralVault.Common.Models.Datafiles
{
    public class VaultFile
    {
        [JsonPropertyName("representation-types")]
        public List<RepresentationType>? RepresentationTypes { get; set; }
    }
}
