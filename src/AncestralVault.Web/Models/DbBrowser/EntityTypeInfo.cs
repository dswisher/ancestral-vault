// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Web.Models.DbBrowser
{
    /// <summary>
    /// Information about an entity type for display on the Home page.
    /// </summary>
    public class EntityTypeInfo
    {
        /// <summary>
        /// The name of the entity type (e.g., "Persona", "DataFile").
        /// </summary>
        public required string TypeName { get; set; }

        /// <summary>
        /// The URL-friendly name for routing (e.g., "persona", "datafile").
        /// </summary>
        public required string UrlName { get; set; }
    }
}
