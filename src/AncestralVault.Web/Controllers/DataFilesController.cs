// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using AncestralVault.Common.Database;
using Microsoft.AspNetCore.Mvc;

namespace AncestralVault.Web.Controllers
{
    public class DataFilesController : Controller
    {
        private readonly IAncestralVaultDbContextFactory contextFactory;

        public DataFilesController(IAncestralVaultDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IActionResult Index()
        {
            using var dbContext = contextFactory.CreateDbContext();
            var dataFiles = dbContext.DataFiles.OrderBy(df => df.RelativePath).ToList();
            return View(dataFiles);
        }
    }
}
