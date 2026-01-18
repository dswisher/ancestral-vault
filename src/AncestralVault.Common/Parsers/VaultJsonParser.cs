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
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Parsers
{
    public class VaultJsonParser : IVaultJsonParser
    {
        private readonly ILogger logger;

        public VaultJsonParser(ILogger<VaultJsonParser> logger)
        {
            this.logger = logger;
        }


        public async Task<List<IVaultJsonEntity>> LoadVaultJsonEntitiesAsync(FileInfo file, bool validateProps, CancellationToken stoppingToken)
        {
            await using (var stream = file.OpenRead())
            {
                // Parse JSON document for validation
                var docOptions = new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip
                };

                using var jsonDoc = await JsonDocument.ParseAsync(stream, docOptions, stoppingToken);

                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };

                options.Converters.Add(new VaultJsonEntityConverter());

                // Deserialize from the JSON document
                var jsonText = jsonDoc.RootElement.GetRawText();
                var entities = JsonSerializer.Deserialize<List<IVaultJsonEntity>>(jsonText, options);

                // Validate for unmapped properties
                if (validateProps && entities != null)
                {
                    var validator = new UnmappedPropertyValidator(logger);
                    validator.Validate(jsonDoc, entities);
                }

                return entities ?? [];
            }
        }


        private sealed class VaultJsonEntityConverter : JsonConverter<IVaultJsonEntity?>
        {
            private static readonly Dictionary<string, Type> TypeMap = new()
            {
                ["census-us-1900"] = typeof(CensusUS1900),
                ["census-us-1930"] = typeof(CensusUS1930),
                ["census-us-1940"] = typeof(CensusUS1940),
                ["composite-persona"] = typeof(JsonCompositePersona),
                ["marriage"] = typeof(JsonMarriage),
                ["persona"] = typeof(JsonPersona),
                ["persona-assertion"] = typeof(JsonPersonaAssertion),
                ["place"] = typeof(JsonPlace),
                ["place-type"] = typeof(JsonPlaceType),
                ["tombstone"] = typeof(JsonTombstone),

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
