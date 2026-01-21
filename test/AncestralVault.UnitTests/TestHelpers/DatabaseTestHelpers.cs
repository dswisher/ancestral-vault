// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common;
using AncestralVault.Common.Database;
using AncestralVault.Common.Loaders;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.Common.Models.VaultJson;
using AncestralVault.Common.Parsers;
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


        public static async Task PopulateDatabaseAsync(ServiceProvider container, string dataFileName, ILogger logger, CancellationToken stoppingToken)
        {
            // Load the entities from the test data file
            var entities = await LoadDataAsync(container, dataFileName, stoppingToken);

            // Grab the DB context
            var dbContext = container.GetRequiredService<AncestralVaultDbContext>();

            // Load some common admin-ish data
            SeedTypes(dbContext);

            // Create a datafile, as we need one for loading almost anything
            var newDataFile = new DataFile
            {
                RelativePath = dataFileName
            };

            dbContext.DataFiles.Add(newDataFile);

            // Load the data into the database
            var loader = container.GetRequiredService<IVaultJsonLoader>();

            loader.LoadEntities(dbContext, newDataFile, entities);

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


        public static void SeedTypes(AncestralVaultDbContext dbContext)
        {
            var newDataFile = AddDataFile(dbContext, "types.jsonc");

            // Load some common admin-ish data
            AddEventType(dbContext, newDataFile, "birth");
            AddEventType(dbContext, newDataFile, "marriage");
            AddEventType(dbContext, newDataFile, "death");
            AddEventType(dbContext, newDataFile, "residence");
            AddEventRoleType(dbContext, newDataFile, "newborn");
            AddEventRoleType(dbContext, newDataFile, "decedent");
            AddEventRoleType(dbContext, newDataFile, "bride");
            AddEventRoleType(dbContext, newDataFile, "groom");
            AddEventRoleType(dbContext, newDataFile, "resident");
        }


        public static ServiceProvider CreateContainer(ITestOutputHelper testOutputHelper)
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

            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            services.AddSingleton(dbContext);

            // Register all the real bits
            services.RegisterLoaders();
            services.RegisterRepositories();

            // Build and return the container
            return services.BuildServiceProvider(validateScopes: true);
        }


        private static void AddEventType(AncestralVaultDbContext context, DataFile dataFile, string eventTypeId)
        {
            var eventType = new EventType
            {
                EventTypeId = eventTypeId,
                Name = eventTypeId,
                DataFile = dataFile
            };

            context.EventTypes.Add(eventType);
        }


        private static void AddEventRoleType(AncestralVaultDbContext context, DataFile dataFile, string eventRoleTypeId)
        {
            var eventRoleType = new EventRoleType
            {
                EventRoleTypeId = eventRoleTypeId,
                Name = eventRoleTypeId,
                DataFile = dataFile
            };

            context.EventRoleTypes.Add(eventRoleType);
        }
    }
}
