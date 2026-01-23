// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Repositories;
using AncestralVault.UnitTests.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AncestralVault.UnitTests.Common.Repositories
{
    public class CompositePersonaRepositoryTests
    {
        private readonly ServiceProvider container;

        private readonly ICompositePersonaRepository personaRepo;
        private readonly AncestralVaultDbContext dbContext;

        private readonly CancellationToken token = CancellationToken.None;


        public CompositePersonaRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            // Set up the mini-container
            container = DatabaseTestHelpers.CreateContainer(testOutputHelper);

            personaRepo = container.GetRequiredService<ICompositePersonaRepository>();
            dbContext = container.GetRequiredService<AncestralVaultDbContext>();
        }


        [Fact]
        public async Task CanLoadUnambiguousComposite()
        {
            // Arrange
            await DatabaseTestHelpers.LoadTestDataFileAsync(container, "test-census-minimal.jsonc", token);
            await DatabaseTestHelpers.LoadTestDataFileAsync(container, "test-marriage-minimal.jsonc", token);
            await DatabaseTestHelpers.LoadTestDataFileAsync(container, "test-tombstone.jsonc", token);

            // TODO - need to add marriage and census persona assertions into this data file
            await DatabaseTestHelpers.LoadTestDataFileAsync(container, "test-composite-minimal.jsonc", token);

            // Act
            var viewModel = await personaRepo.GetPersonaDetailsAsync(dbContext, "walter-smith", token);

            // Assert
            viewModel.Should().NotBeNull();
            viewModel.Name.Should().Be("Walter Smith");

            viewModel.EventBoxItems.Should().Contain(x => x.EventTypeId == "birth");
            viewModel.EventBoxItems.Should().Contain(x => x.EventTypeId == "death");
        }
    }
}
