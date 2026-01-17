// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AncestralVault.Common.Models.VaultJson;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Parsers
{
    public class UnmappedPropertyValidator
    {
        private static readonly Dictionary<Type, HashSet<string>> PropertyNameCache = new();
        private readonly ILogger logger;
        private readonly string[] namespacePatterns;

        public UnmappedPropertyValidator(ILogger logger, params string[] namespacePatterns)
        {
            this.logger = logger;
            this.namespacePatterns = namespacePatterns.Length > 0
                ? namespacePatterns
                : new[] { "AncestralVault.Common.Models.VaultJson" };
        }

        public void Validate(JsonDocument jsonDoc, object obj)
        {
            if (obj == null)
            {
                return;
            }

            ValidateValue(jsonDoc.RootElement, obj, obj.GetType());
        }

        private void ValidateElement(JsonElement jsonElement, object obj, Type type)
        {
            if (jsonElement.ValueKind != JsonValueKind.Object)
            {
                return;
            }

            // Check if this is a VaultJsonEntity wrapper (single property object)
            // If so, unwrap it and validate the inner content
            if (typeof(IVaultJsonEntity).IsAssignableFrom(type))
            {
                var props = jsonElement.EnumerateObject().ToList();
                if (props.Count == 1)
                {
                    // This is a wrapper like {"census-us-1940": {...}}
                    // Unwrap and validate the inner content
                    ValidateElement(props[0].Value, obj, type);
                    return;
                }
            }

            // Check if this type should be validated
            if (!ShouldValidate(type))
            {
                return;
            }

            // Get JSON property names
            var jsonProps = jsonElement.EnumerateObject().Select(p => p.Name).ToHashSet();

            // Get expected C# property names
            var expectedProps = GetExpectedPropertyNames(type);

            // Find and log unmapped properties
            var unmapped = jsonProps.Except(expectedProps).ToList();
            foreach (var propName in unmapped)
            {
                var value = jsonElement.GetProperty(propName).GetRawText();
                logger.LogWarning(
                    "Unmapped property '{PropertyName}' in class {ClassName} with value {Value}",
                    propName, type.FullName ?? type.Name, value);
            }

            // Recursively validate nested objects and collections
            foreach (var jsonProp in jsonElement.EnumerateObject())
            {
                if (expectedProps.Contains(jsonProp.Name))
                {
                    // Find the corresponding C# property
                    var csharpProp = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(p => GetJsonPropertyName(p) == jsonProp.Name);

                    if (csharpProp != null)
                    {
                        var value = csharpProp.GetValue(obj);
                        if (value != null)
                        {
                            ValidateValue(jsonProp.Value, value, csharpProp.PropertyType);
                        }
                    }
                }
            }
        }

        private void ValidateValue(JsonElement jsonElement, object obj, Type type)
        {
            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                ValidateElement(jsonElement, obj, type);
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                // Handle arrays and lists
                if (obj is IEnumerable enumerable)
                {
                    var objEnumerator = enumerable.GetEnumerator();
                    var jsonEnumerator = jsonElement.EnumerateArray().GetEnumerator();

                    while (objEnumerator.MoveNext() && jsonEnumerator.MoveNext())
                    {
                        if (objEnumerator.Current != null)
                        {
                            // Use the actual concrete type of the object, not the collection element type
                            ValidateValue(jsonEnumerator.Current, objEnumerator.Current, objEnumerator.Current.GetType());
                        }
                    }
                }
            }
        }

        private bool ShouldValidate(Type type)
        {
            var typeName = type.FullName ?? type.Name;
            return namespacePatterns.Any(pattern => typeName.StartsWith(pattern, StringComparison.Ordinal));
        }

        private static string GetJsonPropertyName(PropertyInfo prop)
        {
            var attr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
            return attr?.Name ?? prop.Name;
        }

        private static HashSet<string> GetExpectedPropertyNames(Type type)
        {
            if (PropertyNameCache.TryGetValue(type, out var cached))
            {
                return cached;
            }

            var propertyNames = new HashSet<string>(StringComparer.Ordinal);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Check for JsonPropertyName attribute
                var jsonNameAttr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                if (jsonNameAttr != null)
                {
                    propertyNames.Add(jsonNameAttr.Name);
                }
                else
                {
                    propertyNames.Add(prop.Name);
                }

                // Check if property has JsonExtensionData attribute
                var extensionDataAttr = prop.GetCustomAttribute<JsonExtensionDataAttribute>();
                if (extensionDataAttr != null)
                {
                    // This property captures unmapped data, so return empty set to suppress warnings
                    PropertyNameCache[type] = new HashSet<string>();
                    return PropertyNameCache[type];
                }
            }

            PropertyNameCache[type] = propertyNames;
            return propertyNames;
        }
    }
}
