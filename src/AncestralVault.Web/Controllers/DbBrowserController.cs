// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncestralVault.Web.Controllers
{
    /// <summary>
    /// Controller for browsing database entities.
    /// </summary>
    [Route("browse")]
    public class DbBrowserController : Controller
    {
        private readonly IAncestralVaultDbContextFactory contextFactory;
        private readonly IDbBrowserService dbBrowserService;

        public DbBrowserController(IAncestralVaultDbContextFactory contextFactory, IDbBrowserService dbBrowserService)
        {
            this.contextFactory = contextFactory;
            this.dbBrowserService = dbBrowserService;
        }


        /// <summary>
        /// Display all entity types.
        /// </summary>
        /// <returns>An action result.</returns>
        [Route("home")]
        public IActionResult Home()
        {
            var entityTypes = dbBrowserService.GetAllEntityTypes();
            return View(entityTypes);
        }


        /// <summary>
        /// List the first 100 entities of a given type.
        /// </summary>
        /// <param name="type">The type of entities to list.</param>
        /// <returns>An action result.</returns>
        [Route("list/{type}")]
        public IActionResult List(string type)
        {
            if (!dbBrowserService.TryResolveEntityType(type, out var entityType) || entityType == null)
            {
                return NotFound($"Entity type '{type}' not found.");
            }

            using var dbContext = contextFactory.CreateDbContext();
            var viewModel = dbBrowserService.BuildListViewModel(dbContext, entityType);
            return View(viewModel);
        }


        /// <summary>
        /// Display details of a single entity.
        /// </summary>
        /// <param name="type">The type of the entity whose detail is desired.</param>
        /// <param name="id">The id of the entity whose detail is desired.</param>
        /// <returns>An action result.</returns>
        [Route("detail/{type}/{id}")]
        public IActionResult Detail(string type, string id)
        {
            if (!dbBrowserService.TryResolveEntityType(type, out var entityType) || entityType == null)
            {
                return NotFound($"Entity type '{type}' not found.");
            }

            using var dbContext = contextFactory.CreateDbContext();
            var viewModel = dbBrowserService.BuildDetailViewModel(dbContext, entityType, id);

            if (viewModel == null)
            {
                return NotFound($"Entity of type '{type}' with id '{id}' not found.");
            }

            return View(viewModel);
        }
    }
}
