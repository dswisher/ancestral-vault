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
using AncestralVault.Cli.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace AncestralVault.Cli.Commands
{
    public class LoadFileCommand
    {
        private readonly IVaultJsonParser parser;
        private readonly IVaultJsonLoader loader;
        private readonly ILogger<LoadFileCommand> logger;

        public LoadFileCommand(
            IVaultJsonParser parser,
            IVaultJsonLoader loader,
            ILogger<LoadFileCommand> logger)
        {
            this.parser = parser;
            this.loader = loader;
            this.logger = logger;
        }


        public async Task ExecuteAsync(LoadFileOptions options, CancellationToken stoppingToken)
        {
            var timer = Stopwatch.StartNew();

            // Figure out the vault path
            var vaultDir = new DirectoryInfo(options.VaultPath ?? "../../../family");
            var dbDir = new DirectoryInfo(Path.Join(vaultDir.FullName, ".db"));
            var vaultPath = new FileInfo(Path.Join(dbDir.FullName, "vault.db"));

            // Ensure database exists
            if (!vaultPath.Exists)
            {
                logger.LogError("Database not found at: {VaultPath}. Run 'rebuild' command first.", vaultPath.FullName);
                return;
            }

            // Find the file in vault
            logger.LogInformation("Searching for data file: {DataFile}", options.DataFile);
            var isDirectoryPath = ContainsDirectoryPath(options.DataFile);
            var matchingFiles = vaultDir
                .EnumerateFiles("*.jsonc", SearchOption.AllDirectories)
                .Where(f => FileMatchesSearchPath(f, vaultDir, options.DataFile, isDirectoryPath))
                .ToList();

            if (matchingFiles.Count == 0)
            {
                logger.LogError("Data file '{DataFile}' not found in vault.", options.DataFile);
                return;
            }

            if (matchingFiles.Count > 1)
            {
                logger.LogError("Multiple files named '{DataFile}' found in vault:", options.DataFile);
                foreach (var file in matchingFiles)
                {
                    logger.LogError("  - {FilePath}", Path.GetRelativePath(vaultDir.FullName, file.FullName));
                }
                return;
            }

            var dataFile = matchingFiles[0];
            var relativePath = Path.GetRelativePath(vaultDir.FullName, dataFile.FullName).Replace('\\', '/');
            logger.LogInformation("Found data file at: {RelativePath}", relativePath);

            // Open database and process
            var optionsBuilder = new DbContextOptionsBuilder<AncestralVaultDbContext>()
                .UseSqlite($"Data Source={vaultPath.FullName}")
                .ReplaceService<IMigrationsSqlGenerator, DeferrableForeignKeysSqlGenerator>();

            await using (var context = new AncestralVaultDbContext(optionsBuilder.Options))
            {
                // Look up existing DataFile record
                var existingDataFile = await context.DataFiles
                    .FirstOrDefaultAsync(df => df.RelativePath == relativePath, stoppingToken);

                if (existingDataFile != null)
                {
                    logger.LogInformation("Data file previously loaded, deleting existing records...");

                    // Remove the DataFile record - cascade delete will handle child records
                    context.DataFiles.Remove(existingDataFile);
                    await context.SaveChangesAsync(stoppingToken);
                }
                else
                {
                    logger.LogInformation("Data file not previously loaded, performing initial load.");
                }

                // Parse the file
                logger.LogInformation("Parsing data file {FileName}...", relativePath);
                var vaultEntities = await parser.LoadVaultJsonEntitiesAsync(dataFile, options.CheckProps, stoppingToken);

                if (vaultEntities.Count == 0)
                {
                    logger.LogWarning("No entities found in data file {FileName}.", relativePath);
                    return;
                }

                // Create new DataFile entry
                var newDataFile = new DataFile
                {
                    RelativePath = relativePath
                };
                context.DataFiles.Add(newDataFile);

                // Load all entities
                foreach (var entity in vaultEntities)
                {
                    logger.LogDebug("Processing entity of type {EntityType}...", entity.GetType().Name);
                    loader.LoadEntity(context, newDataFile, entity);
                }

                // Save changes
                logger.LogInformation("Saving changes to database...");
                await context.SaveChangesAsync(stoppingToken);
            }

            logger.LogInformation("Load complete in {Elapsed}.", timer.Elapsed);
        }


        private static string NormalizePathSeparators(string path)
        {
            return path.Replace('\\', '/');
        }


        private static bool ContainsDirectoryPath(string dataFile)
        {
            return dataFile.Contains('/') || dataFile.Contains('\\');
        }


        private static bool FileMatchesSearchPath(
            FileInfo file,
            DirectoryInfo vaultDir,
            string searchPath,
            bool isDirectoryPath)
        {
            if (isDirectoryPath)
            {
                // Match against full relative path
                string relativePath = Path.GetRelativePath(vaultDir.FullName, file.FullName)
                    .Replace('\\', '/');

                string normalizedSearchPath = NormalizePathSeparators(searchPath);

                return relativePath.EndsWith(
                    normalizedSearchPath,
                    System.StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Match only filename (existing behavior)
                return file.Name.Equals(
                    searchPath,
                    System.StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
