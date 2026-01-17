// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics;
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
            // Keep track of the time
            var timer = Stopwatch.StartNew();

            // Figure out the vault path
            // TODO - "hunt" for the directory, if one is not specified
            var vaultDir = new DirectoryInfo("../../../family");
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
                    await LoadData(context, options, vaultDir, stoppingToken);
                }
            }

            // Report!
            logger.LogInformation("Rebuild complete in {Elapsed}.", timer.Elapsed);
        }


        private async Task LoadData(AncestralVaultDbContext context, RebuildOptions options, DirectoryInfo vaultDir, CancellationToken stoppingToken)
        {
            // Load all the data
            // TODO - also scan for .json files!
            foreach (var file in vaultDir.EnumerateFiles("*.jsonc", SearchOption.AllDirectories).OrderBy(x => x.FullName))
            {
                var relativePath = Path.GetRelativePath(vaultDir.FullName, file.FullName).Replace('\\', '/');

                // TODO - load all the data
                logger.LogInformation("Parsing data file {FileName}...", relativePath);
                var vaultEntities = await parser.LoadVaultJsonEntitiesAsync(file, options.ValidateProps, stoppingToken);

                if (vaultEntities.Count == 0)
                {
                    logger.LogWarning("...no entities found in data file {FileName}.", relativePath);
                    continue;
                }

                // Create a data file entry
                var dataFile = new DataFile
                {
                    RelativePath = relativePath
                };

                context.DataFiles.Add(dataFile);

                // Process all the items in the file
                foreach (var entity in vaultEntities)
                {
                    logger.LogDebug("   -> processing entity of type {EntityType}...", entity.GetType().Name);

                    loader.LoadEntity(context, dataFile, entity);
                }

                // Save changes after each file
                logger.LogDebug("...saving changes to database after processing file {FileName}...", relativePath);
                await context.SaveChangesAsync(stoppingToken);
            }

            // Make sure all work has been saved
            logger.LogInformation("Saving changes to database...");

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
