// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels.DbBrowser;

namespace AncestralVault.Common.Repositories
{
    /// <summary>
    /// Service for browsing database entities using reflection.
    /// </summary>
    public interface IDbBrowserRepository
    {
        /// <summary>
        /// Get all entity types available in the database.
        /// </summary>
        /// <returns>List of entity type information.</returns>
        List<EntityTypeInfo> GetAllEntityTypes();

        /// <summary>
        /// Try to resolve an entity type from a URL-friendly name.
        /// </summary>
        /// <param name="typeName">The type name (case-insensitive, may be plural).</param>
        /// <param name="entityType">The resolved Type if found.</param>
        /// <returns>True if the type was found, false otherwise.</returns>
        bool TryResolveEntityType(string typeName, out Type? entityType);

        /// <summary>
        /// Build the view model for the list page.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityType">The entity type to list.</param>
        /// <returns>The view model for the list page.</returns>
        EntityListViewModel BuildListViewModel(AncestralVaultDbContext dbContext, Type entityType);

        /// <summary>
        /// Build the view model for the detail page.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="id">The entity ID.</param>
        /// <returns>The view model for the detail page, or null if not found.</returns>
        EntityDetailViewModel? BuildDetailViewModel(AncestralVaultDbContext dbContext, Type entityType, string id);
    }
}
