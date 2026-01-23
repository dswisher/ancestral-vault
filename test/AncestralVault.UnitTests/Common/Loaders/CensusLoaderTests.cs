// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Loaders;
using AncestralVault.Common.Models.VaultDb;
using AncestralVault.UnitTests.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AncestralVault.UnitTests.Common.Loaders
{
    public class CensusLoaderTests
    {
        private readonly ServiceProvider container;

        private readonly IVaultJsonLoader loader;
        private readonly AncestralVaultDbContext dbContext;
        private readonly DataFile dataFile;

        private readonly CancellationToken token = CancellationToken.None;


        public CensusLoaderTests(ITestOutputHelper testOutputHelper)
        {
            // Set up the mini-container and pull out required services
            container = DatabaseTestHelpers.CreateContainer(testOutputHelper);

            loader = container.GetRequiredService<IVaultJsonLoader>();
            dbContext = container.GetRequiredService<AncestralVaultDbContext>();

            // Create a data file
            dataFile = DatabaseTestHelpers.AddDataFile(dbContext, "foo.jsonc");

            // Save it all
            dbContext.SaveChanges();
        }


        [Fact]
        public async Task CanLoadSimpleCensus()
        {
            // Arrange
            var entities = await DatabaseTestHelpers.LoadDataAsync(container, "test-census-minimal.jsonc", token);

            // Act
            await loader.LoadEntitiesAsync(dbContext, dataFile, entities, token);

            await dbContext.SaveChangesAsync(token);

            // Assert
            dbContext.Personas.Should().HaveCount(4);

            // TODO - check persona details and check events/roles
        }
    }
}
