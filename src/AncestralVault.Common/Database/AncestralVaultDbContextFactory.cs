// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Common.Database
{
    public class AncestralVaultDbContextFactory : IAncestralVaultDbContextFactory
    {
        private readonly IVaultSeeker seeker;
        private readonly ILogger<AncestralVaultDbContext> logger;

        public AncestralVaultDbContextFactory(
            IVaultSeeker seeker,
            ILogger<AncestralVaultDbContext> logger)
        {
            this.seeker = seeker;
            this.logger = logger;
        }


        public AncestralVaultDbContext CreateDbContext()
        {
            logger.LogInformation("Opening database at: {VaultPath}", seeker.VaultDbFile.FullName);

            var optionsBuilder = new DbContextOptionsBuilder<AncestralVaultDbContext>();
            optionsBuilder.UseSqlite($"Data Source={seeker.VaultDbFile}")
                .ReplaceService<IMigrationsSqlGenerator, DeferrableForeignKeysSqlGenerator>();

            return new AncestralVaultDbContext(optionsBuilder.Options);
        }
    }
}
