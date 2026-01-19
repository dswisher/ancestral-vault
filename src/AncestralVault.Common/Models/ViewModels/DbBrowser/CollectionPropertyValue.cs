// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;

namespace AncestralVault.Common.Models.ViewModels.DbBrowser
{
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
