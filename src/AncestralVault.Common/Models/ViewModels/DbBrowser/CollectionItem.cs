// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Common.Models.ViewModels.DbBrowser
{
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
}
