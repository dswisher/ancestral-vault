// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;

namespace AncestralVault.Common.Models.ViewModels.DbBrowser
{
    /// <summary>
    /// The type of property, which determines how it should be rendered.
    /// </summary>
    public enum PropertyType
    {
        /// <summary>
        /// Simple value type or string - display as text.
        /// </summary>
        Simple,

        /// <summary>
        /// Foreign key property - display as a link if not null.
        /// </summary>
        ForeignKey,

        /// <summary>
        /// Navigation property to a single related entity.
        /// </summary>
        ForeignKeyNavigation,

        /// <summary>
        /// Collection navigation property.
        /// </summary>
        Collection
    }

    /// <summary>
    /// Represents a property and its value for display.
    /// </summary>
    public class PropertyValue
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The value of the property (for display).
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// The type of property (determines how to render it).
        /// </summary>
        public PropertyType Type { get; set; }

        /// <summary>
        /// For foreign key navigations and collections, the entity type name.
        /// </summary>
        public string? RelatedEntityType { get; set; }

        /// <summary>
        /// For collections, the list of related entity IDs.
        /// </summary>
        public System.Collections.Generic.List<string>? RelatedEntityIds { get; set; }
    }

    /// <summary>
    /// Represents a single item in a collection with its property values.
    /// </summary>
    public class CollectionItem
    {
        /// <summary>
        /// The primary key value (as string for URL routing).
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Key-value pairs of property names and values for display.
        /// </summary>
        public required System.Collections.Generic.Dictionary<string, object?> Properties { get; set; }
    }

    /// <summary>
    /// Represents a collection property for separate display.
    /// </summary>
    public class CollectionPropertyValue
    {
        /// <summary>
        /// The name of the collection property.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The entity type name of items in the collection.
        /// </summary>
        public required string RelatedEntityType { get; set; }

        /// <summary>
        /// The list of items in the collection with their property values.
        /// </summary>
        public required System.Collections.Generic.List<CollectionItem> Items { get; set; }

        /// <summary>
        /// Backward compatibility: computed from Items.
        /// </summary>
        public System.Collections.Generic.List<string> RelatedEntityIds => Items.Select(i => i.Id).ToList();
    }
}
