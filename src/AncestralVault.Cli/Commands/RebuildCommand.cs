// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Cli.Options;
using AncestralVault.Common.Assistants.Places;
using AncestralVault.Common.Database;
using AncestralVault.Common.Loaders;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Parsers;
using AncestralVault.Common.Utilities;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Cli.Commands
{
    public class RebuildCommand
    {
        private readonly IVaultSeeker seeker;
        private readonly ITypePopulator typePopulator;
        private readonly IVaultJsonParser parser;
        private readonly IVaultJsonLoader loader;
        private readonly IAncestralVaultDbContextFactory dbContextFactory;
        private readonly IPlaceCache placeCache;
        private readonly ILogger<RebuildCommand> logger;

        public RebuildCommand(
            IVaultSeeker seeker,
            ITypePopulator typePopulator,
            IVaultJsonParser parser,
            IVaultJsonLoader loader,
            IAncestralVaultDbContextFactory dbContextFactory,
            IPlaceCache placeCache,
            ILogger<RebuildCommand> logger)
        {
            this.seeker = seeker;
            this.typePopulator = typePopulator;
            this.parser = parser;
            this.loader = loader;
            this.dbContextFactory = dbContextFactory;
            this.placeCache = placeCache;
            this.logger = logger;
        }


        public async Task ExecuteAsync(RebuildOptions options, CancellationToken stoppingToken)
        {
            // Keep track of the time
            var timer = Stopwatch.StartNew();

            // Set up the vault info
            seeker.Configure(options.VaultPath);

            // Make sure the DB directory exists
            if (!seeker.VaultDbDir.Exists)
            {
                logger.LogInformation("Creating database directory at: {DbDir}", seeker.VaultDbDir.FullName);
                seeker.VaultDbDir.Create();
            }

            // Connect to the database
            await using (var context = dbContextFactory.CreateDbContext())
            {
                // Recreate the database
                logger.LogInformation("...ensuring database is deleted...");
                await context.Database.EnsureDeletedAsync(stoppingToken);

                logger.LogInformation("...ensuring database is created...");
                await context.Database.EnsureCreatedAsync(stoppingToken);

                // If they want to load the data, do so.
                if (!options.SchemaOnly)
                {
                    await LoadData(context, options, seeker.VaultDir, stoppingToken);
                }
            }

            // Report!
            logger.LogInformation("Rebuild complete in {Elapsed}.", timer.Elapsed);
        }


        private async Task LoadData(AncestralVaultDbContext context, RebuildOptions options, DirectoryInfo vaultDir, CancellationToken stoppingToken)
        {
            // Populate all the types
            typePopulator.PopulateAllTypes(context);

            // Load directories in a specific order, so prerequisites are loaded before the things
            // that depend on them. After places are loaded, populate the place cache.
            await LoadDirectoryData(context, options, vaultDir, "places", stoppingToken);

            await placeCache.RefreshCacheIfNeededAsync(context, stoppingToken);

            await LoadDirectoryData(context, options, vaultDir, "evidence", stoppingToken);
            await LoadDirectoryData(context, options, vaultDir, "conclusions", stoppingToken);
        }


        private async Task LoadDirectoryData(AncestralVaultDbContext context, RebuildOptions options, DirectoryInfo vaultDir, string directoryName, CancellationToken stoppingToken)
        {
            // Get the directory info, and make sure it exists
            var directoryInfo = new DirectoryInfo(Path.Join(vaultDir.FullName, directoryName));
            if (!directoryInfo.Exists)
            {
                return;
            }

            logger.LogDebug("Loading data from directory: {DirectoryName}...", directoryInfo.FullName);

            // Load all the data
            // TODO - also scan for .json files!
            foreach (var file in directoryInfo.EnumerateFiles("*.jsonc", SearchOption.AllDirectories).OrderBy(x => x.FullName))
            {
                var relativePath = Path.GetRelativePath(directoryInfo.FullName, file.FullName).Replace('\\', '/');

                // TODO - load all the data
                logger.LogInformation("Parsing data file {FileName}...", $"{directoryName}/{relativePath}");
                var vaultEntities = await parser.LoadVaultJsonEntitiesAsync(file, options.CheckProps, stoppingToken);

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
                await loader.LoadEntitiesAsync(context, dataFile, vaultEntities, stoppingToken);

                // Save changes after each file
                logger.LogDebug("...saving changes to database after processing file {FileName}...", relativePath);
                await context.SaveChangesAsync(stoppingToken);
            }

            // Make sure all work has been saved
            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
