// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Loaders;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Parsers;
using AncestralVault.TestCli.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace AncestralVault.TestCli.Commands
{
    public class RebuildCommand
    {
        private readonly IVaultJsonParser parser;
        private readonly IVaultJsonLoader loader;
        private readonly ILogger<RebuildCommand> logger;

        public RebuildCommand(
            IVaultJsonParser parser,
            IVaultJsonLoader loader,
            ILogger<RebuildCommand> logger)
        {
            this.parser = parser;
            this.loader = loader;
            this.logger = logger;
        }


        public async Task ExecuteAsync(RebuildOptions options, CancellationToken stoppingToken)
        {
            // Figure out the vault path
            // TODO - "hunt" for the directory, if one is not specified
            var vaultDir = new DirectoryInfo("../../sample-vault");
            var dbDir = new DirectoryInfo(Path.Join(vaultDir.FullName, ".db"));
            var vaultPath = new FileInfo(Path.Join(dbDir.FullName, "vault.db"));

            // Make sure the DB directory exists
            if (!dbDir.Exists)
            {
                logger.LogInformation("Creating database directory at: {DbDir}", dbDir.FullName);
                dbDir.Create();
            }

            // TODO - we should be getting the DB context from DI
            logger.LogInformation("Opening database at: {VaultPath}", vaultPath.FullName);
            var optionsBuilder = new DbContextOptionsBuilder<AncestralVaultDbContext>()
                .UseSqlite($"Data Source={vaultPath.FullName}")
                .ReplaceService<IMigrationsSqlGenerator, DeferrableForeignKeysSqlGenerator>();

            await using (var context = new AncestralVaultDbContext(optionsBuilder.Options))
            {
                // Recreate the database
                logger.LogInformation("...ensuring database is deleted...");
                await context.Database.EnsureDeletedAsync(stoppingToken);

                logger.LogInformation("...ensuring database is created...");
                await context.Database.EnsureCreatedAsync(stoppingToken);

                // If they want to load the data, do so.
                if (!options.SchemaOnly)
                {
                    await LoadData(context, vaultDir, stoppingToken);
                }
            }
        }


        private async Task LoadData(AncestralVaultDbContext context, DirectoryInfo vaultDir, CancellationToken stoppingToken)
        {
            // Start a database transaction
            // TODO - start a transaction?

            // Load all the data
            foreach (var file in vaultDir.EnumerateFiles("*.json", SearchOption.AllDirectories).OrderBy(x => x.FullName))
            {
                // TODO - load all the data
                logger.LogInformation("Parsing data file {FileName}...", file.Name);
                var vaultEntities = await parser.LoadVaultJsonEntitiesAsync(file, stoppingToken);

                if (vaultEntities.Count == 0)
                {
                    logger.LogWarning("...no entities found in data file {FileName}.", file.Name);
                    continue;
                }

                // Create a data file entry
                var dataFile = new DataFile
                {
                    RelativePath = Path.GetRelativePath(vaultDir.FullName, file.FullName).Replace('\\', '/')
                };

                context.DataFiles.Add(dataFile);

                // Process all the items in the file
                foreach (var entity in vaultEntities)
                {
                    logger.LogInformation("   -> processing entity of type {EntityType}...", entity.GetType().Name);

                    loader.LoadEntity(context, dataFile, entity);
                }
            }

            // Commit the transaction
            // TODO - commit the transaction?
            logger.LogInformation("Saving changes to database...");

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
