// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace AncestralVault.Common.Models.ViewModels.DbBrowser
{
    /// <summary>
    /// View model for the List page showing entities of a specific type.
    /// </summary>
    public class EntityListViewModel
    {
        /// <summary>
        /// The name of the entity type.
        /// </summary>
        public required string TypeName { get; set; }

        /// <summary>
        /// The URL-friendly type name.
        /// </summary>
        public required string UrlName { get; set; }

        /// <summary>
        /// The name of the primary key property.
        /// </summary>
        public required string PrimaryKeyName { get; set; }

        /// <summary>
        /// The list of entities with their property values.
        /// </summary>
        public required List<EntityListItem> Entities { get; set; }
    }
}
