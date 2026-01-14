// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Models.VaultJson;
using AncestralVault.Common.Models.VaultJson.CensusUS;

namespace AncestralVault.Common.Parsers
{
    public class VaultJsonParser : IVaultJsonParser
    {
        public async Task<List<IVaultJsonEntity>> LoadVaultJsonEntitiesAsync(FileInfo file, CancellationToken stoppingToken)
        {
            await using (var stream = file.OpenRead())
            {
                var options = new JsonSerializerOptions();

                options.Converters.Add(new VaultJsonEntityConverter());

                var entities = await JsonSerializer.DeserializeAsync<List<IVaultJsonEntity>>(stream, options, stoppingToken);

                return entities ?? new List<IVaultJsonEntity>();
            }
        }


        private sealed class VaultJsonEntityConverter : JsonConverter<IVaultJsonEntity?>
        {
            private static readonly Dictionary<string, Type> TypeMap = new()
            {
                ["census-us-1930"] = typeof(CensusUS1930),
                ["persona"] = typeof(JsonPersona),

                // ["project"] = typeof(Project),
                // ["researcher"] = typeof(Researcher),
                // ["activity"] = typeof(Activity),
                // ["event"] = typeof(Event)
                // ... register all types
            };

            private static readonly Dictionary<Type, string> NameMap =
                TypeMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

            public override IVaultJsonEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;
                var property = root.EnumerateObject().First();

                if (!TypeMap.TryGetValue(property.Name, out var targetType))
                {
                    throw new JsonException($"Unknown entity type: {property.Name}");
                }

                var content = property.Value.GetRawText();
                return (IVaultJsonEntity?)JsonSerializer.Deserialize(content, targetType, options);
            }


            public override void Write(Utf8JsonWriter writer, IVaultJsonEntity? value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                    return;
                }

                if (!NameMap.TryGetValue(value.GetType(), out var typeName))
                {
                    throw new JsonException($"Unregistered entity type: {value.GetType().Name}");
                }

                writer.WriteStartObject();
                writer.WritePropertyName(typeName);
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
                writer.WriteEndObject();
            }
        }
    }
}
