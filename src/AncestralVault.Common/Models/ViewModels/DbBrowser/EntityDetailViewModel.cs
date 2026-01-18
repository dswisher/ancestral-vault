// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace AncestralVault.Common.Models.ViewModels.DbBrowser
{
    /// <summary>
    /// View model for the Detail page showing a single entity.
    /// </summary>
    public class EntityDetailViewModel
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
        /// The primary key value.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// All properties of the entity with their values and metadata.
        /// </summary>
        public required List<PropertyValue> Properties { get; set; }

        /// <summary>
        /// Collection properties that should be displayed in separate tables.
        /// </summary>
        public required List<CollectionPropertyValue> Collections { get; set; }
    }
}
