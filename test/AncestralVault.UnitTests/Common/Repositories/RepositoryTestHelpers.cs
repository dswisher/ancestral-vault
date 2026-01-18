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
using AncestralVault.UnitTests.TestHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AncestralVault.UnitTests.Common.Repositories
{
    public static class RepositoryTestHelpers
    {
        public static async Task PopulateDatabaseAsync(ServiceProvider container, string dataFileName, CancellationToken stoppingToken)
        {
            // Load the entities from the test data file
            var path = $"AncestralVault.UnitTests.Common.Repositories.TestData.{dataFileName}";
            List<IVaultJsonEntity> entities;
            await using (var stream = typeof(PersonaRepositoryTests).Assembly.GetManifestResourceStream(path))
            {
                stream.Should().NotBeNull();

                var parser = container.GetRequiredService<IVaultJsonParser>();
                entities = await parser.LoadVaultJsonEntitiesAsync(stream, validateProps: true, stoppingToken);

                entities.Should().NotBeNullOrEmpty();
            }

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
            foreach (var entity in entities)
            {
                loader.LoadEntity(dbContext, newDataFile, entity);
            }

            // Save all the changes
            await dbContext.SaveChangesAsync(stoppingToken);
        }


        public static ServiceProvider CreateContainer(ITestOutputHelper testOutputHelper)
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Set up test-output based logging
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddProvider(new TestOutputLoggerProvider(testOutputHelper));
            });

            // Set up the database context, and register it
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
    }
}
