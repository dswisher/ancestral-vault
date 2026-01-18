// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace AncestralVault.Common.Models.ViewModels.DbBrowser
{
    /// <summary>
    /// Represents a single entity in the list.
    /// </summary>
    public class EntityListItem
    {
        /// <summary>
        /// The primary key value (as string for URL routing).
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Key-value pairs of property names and values for display.
        /// </summary>
        public required Dictionary<string, object?> Properties { get; set; }
    }
}
