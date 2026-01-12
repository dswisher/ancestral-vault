using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.Database;
using AncestralVault.Common.Models.Datafiles;
using AncestralVault.TestCli.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace AncestralVault.TestCli.Commands
{
    public class RebuildCommand
    {
        private readonly ILogger<RebuildCommand> logger;

        public RebuildCommand(ILogger<RebuildCommand> logger)
        {
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

                // Start a database transaction
                // TODO - start a transaction

                // Load all the data
                foreach (var file in vaultDir.EnumerateFiles("*.json", SearchOption.AllDirectories).OrderBy(x => x.FullName))
                {
                    // TODO - load all the data
                    logger.LogInformation("Loading data file {FileName}...", file.Name);
                    await using (var stream = file.OpenRead())
                    {
                        var vaultFile = await JsonSerializer.DeserializeAsync<VaultFile>(stream, cancellationToken: stoppingToken);

                        if (vaultFile == null)
                        {
                            logger.LogWarning("...failed to deserialize data file {FileName}.", file.Name);
                            continue;
                        }

                        logger.LogInformation("Loaded {NumRepTypes} representation types and {NumReps} representations.",
                            vaultFile.RepresentationTypes?.Count ?? 0, vaultFile.Representations?.Count ?? 0);

                        // Create a data file entry
                        var dataFile = new DataFile
                        {
                            RelativePath = Path.GetRelativePath(vaultDir.FullName, file.FullName).Replace('\\', '/')
                        };

                        context.DataFiles.Add(dataFile);

                        // Add representation types to the database
                        if (vaultFile.RepresentationTypes != null)
                        {
                            foreach (var repType in vaultFile.RepresentationTypes)
                            {
                                logger.LogInformation("...adding representation type {RepTypeId} ({RepTypeName})...", repType.RepresentationTypeId, repType.Name);

                                repType.DataFile = dataFile;

                                context.RepresentationTypes.Add(repType);
                            }
                        }

                        // Add representations to the database
                        if (vaultFile.Representations != null)
                        {
                            foreach (var rep in vaultFile.Representations)
                            {
                                logger.LogInformation("...adding representation {FileCode}...", rep.PhysicalFileCode);

                                rep.DataFile = dataFile;

                                context.Representations.Add(rep);
                            }
                        }
                    }
                }

                // Commit the transaction
                logger.LogInformation("Saving changes to database...");

                // TODO - commit the transaction
                await context.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
