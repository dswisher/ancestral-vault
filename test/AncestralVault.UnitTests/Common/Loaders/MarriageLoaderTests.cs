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
    public class MarriageLoaderTests
    {
        private readonly ServiceProvider container;

        private readonly IVaultJsonLoader loader;
        private readonly AncestralVaultDbContext dbContext;
        private readonly DataFile dataFile;

        private readonly CancellationToken token = CancellationToken.None;


        public MarriageLoaderTests(ITestOutputHelper testOutputHelper)
        {
            // Set up the mini-container
            container = DatabaseTestHelpers.CreateContainer(testOutputHelper);

            loader = container.GetRequiredService<IVaultJsonLoader>();
            dbContext = container.GetRequiredService<AncestralVaultDbContext>();

            // Load some common admin-ish data
            DatabaseTestHelpers.SeedTypes(dbContext);

            // Set up the data file
            dataFile = new DataFile
            {
                RelativePath = "foo.jsonc"
            };

            dbContext.DataFiles.Add(dataFile);

            // Save it all
            dbContext.SaveChanges();
        }


        [Fact]
        public async Task CanLoadSimpleMarriage()
        {
            // Arrange
            var entities = await DatabaseTestHelpers.LoadDataAsync(container, "test-marriage-minimal.jsonc", token);

            // Act
            loader.LoadEntities(dbContext, dataFile, entities);

            await dbContext.SaveChangesAsync(token);

            // Assert
            dbContext.Personas.Should().HaveCount(2);

            // TODO - check persona details and check events/roles
        }
    }
}
