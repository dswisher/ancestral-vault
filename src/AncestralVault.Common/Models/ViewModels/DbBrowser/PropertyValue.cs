// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
}
