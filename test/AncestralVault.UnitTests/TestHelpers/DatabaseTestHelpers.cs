// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common;
using AncestralVault.Common.Assistants.Places;
using AncestralVault.Common.Database;
using AncestralVault.Common.Loaders;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;
using AncestralVault.Common.Parsers;
using AncestralVault.UnitTests.Common.Assistants.Places;
using AncestralVault.UnitTests.Common.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AncestralVault.UnitTests.TestHelpers
{
    public static class DatabaseTestHelpers
    {
        public static async Task<List<IVaultJsonEntity>> LoadDataAsync(ServiceProvider container, string dataFileName, CancellationToken stoppingToken)
        {
            var path = $"AncestralVault.UnitTests.TestHelpers.TestData.{dataFileName}";
            List<IVaultJsonEntity> entities;
            await using (var stream = typeof(PersonaRepositoryTests).Assembly.GetManifestResourceStream(path))
            {
                stream.Should().NotBeNull();

                var parser = container.GetRequiredService<IVaultJsonParser>();
                entities = await parser.LoadVaultJsonEntitiesAsync(stream, validateProps: true, stoppingToken);

                entities.Should().NotBeNullOrEmpty();
            }

            return entities;
        }


        public static async Task LoadTestDataFileAsync(ServiceProvider container, string dataFileName, CancellationToken stoppingToken)
        {
            // Load the entities from the test data file
            var entities = await LoadDataAsync(container, dataFileName, stoppingToken);

            // Grab the DB context
            var dbContext = container.GetRequiredService<AncestralVaultDbContext>();

            // Create a datafile, as we need one for loading almost anything
            var newDataFile = new DataFile
            {
                RelativePath = dataFileName
            };

            dbContext.DataFiles.Add(newDataFile);

            // Load the data into the database
            var loader = container.GetRequiredService<IVaultJsonLoader>();

            await loader.LoadEntitiesAsync(dbContext, newDataFile, entities, stoppingToken);

            // Save all the changes
            await dbContext.SaveChangesAsync(stoppingToken);
        }


        public static DataFile AddDataFile(AncestralVaultDbContext dbContext, string relativePath)
        {
            var dataFile = new DataFile
            {
                RelativePath = relativePath
            };

            dbContext.DataFiles.Add(dataFile);

            return dataFile;
        }


        public static async Task<ServiceProvider> CreateContainerAsync(ITestOutputHelper testOutputHelper, CancellationToken stoppingToken)
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Create a logger factory
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddProvider(new TestOutputLoggerProvider(testOutputHelper));
            });

            // Register the logger factory in the container
            services.AddSingleton(loggerFactory);
            services.AddLogging();

            // Set up the database context, and register it
            // To enable logging, add this line to the code below. Note, however, that it creates a TON of messages.
            //      .UseLoggerFactory(loggerFactory)
            var options = new DbContextOptionsBuilder<AncestralVaultDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var dbContext = new AncestralVaultDbContext(options);

            await dbContext.Database.OpenConnectionAsync(stoppingToken);
            await dbContext.Database.EnsureCreatedAsync(stoppingToken);

            await dbContext.SaveChangesAsync(stoppingToken);

            services.AddSingleton(dbContext);

            // Register all the real bits
            services.RegisterAssistants();
            services.RegisterLoaders();
            services.RegisterRepositories();

            // Build and return the container
            var container =  services.BuildServiceProvider(validateScopes: true);

            await SeedTypesAndPlacesAsync(container, dbContext, stoppingToken);

            return container;
        }


        public static async Task DisableForeignKeysAsync(AncestralVaultDbContext dbContext, CancellationToken stoppingToken)
        {
            await using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "PRAGMA foreign_keys = false;";
                await command.ExecuteNonQueryAsync(stoppingToken);
            }
        }


        public static async Task EnableForeignKeysAsync(AncestralVaultDbContext dbContext, CancellationToken stoppingToken)
        {
            await using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "PRAGMA foreign_keys = true;";
                await command.ExecuteNonQueryAsync(stoppingToken);
            }
        }


        public static async Task VerifyForeignKeysAsync(AncestralVaultDbContext dbContext, CancellationToken stoppingToken)
        {
            var builder = new StringBuilder();

            await using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "PRAGMA foreign_key_check;";

                await using (var reader = await command.ExecuteReaderAsync(stoppingToken))
                {
                    while (await reader.ReadAsync(stoppingToken))
                    {
                        var table = reader.GetString(0);
                        var rowId = reader.GetInt32(1);
                        var fkIndex = reader.GetInt32(2);
                        var fkValue = reader.GetValue(3);

                        builder.AppendLine($"FK Violation in table '{table}', row ID {rowId}, FK index {fkIndex}, FK value '{fkValue}'");
                    }
                }
            }

            builder.ToString().Should().BeEmpty();
        }


        private static void LoadTestPlaces(AncestralVaultDbContext dbContext)
        {
            var newDataFile = new DataFile
            {
                RelativePath = "places.csv"
            };

            dbContext.DataFiles.Add(newDataFile);

            const string path = "AncestralVault.UnitTests.TestHelpers.TestData.places.csv";
            using (var stream = typeof(PlaceNameParserTests).Assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream!))
            {
                var row = 0;
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    row += 1;
                    if (row == 1)
                    {
                        // Skip header line
                        continue;
                    }

                    // Parse the line and create the place
                    var parts = line.Split(',');

                    var placeId = parts[0];
                    var placeTypeId = parts[1];
                    var placeName = parts[2];
                    var parentPlaceId = parts.Length > 3 ? parts[3] : null;

                    // TODO - load abbreviations, if present
                    var abbreviations = parts.Length > 4 ? parts[4].Split('|') : [];

                    var place = new Place
                    {
                        PlaceId = placeId,
                        PlaceTypeId = placeTypeId,
                        Name = placeName,
                        DataFile = newDataFile
                    };

                    if (!string.IsNullOrEmpty(parentPlaceId))
                    {
                        place.ParentPlaceId = parentPlaceId;
                    }

                    // Add the place to the DB context
                    dbContext.Places.Add(place);
                }
            }
        }


        private static async Task SeedTypesAndPlacesAsync(ServiceProvider container, AncestralVaultDbContext dbContext, CancellationToken stoppingToken)
        {
            var populator = container.GetRequiredService<ITypePopulator>();
            var placeCache = container.GetRequiredService<IPlaceCache>();

            populator.PopulateAllTypes(dbContext);

            // await dbContext.SaveChangesAsync(stoppingToken);
            // await DisableForeignKeysAsync(dbContext, stoppingToken);

            LoadTestPlaces(dbContext);

            await dbContext.SaveChangesAsync(stoppingToken);

            // await VerifyForeignKeysAsync(dbContext, stoppingToken);
            // await EnableForeignKeysAsync(dbContext, stoppingToken);

            await placeCache.RefreshCacheIfNeededAsync(dbContext, stoppingToken);
        }
    }
}
