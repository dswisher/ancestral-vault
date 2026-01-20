// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Repositories;
using AncestralVault.UnitTests.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace AncestralVault.UnitTests.Common.Repositories
{
    public class PersonaRepositoryTests : IDisposable
    {
        private readonly ServiceProvider container;

        private readonly IPersonaRepository personaRepo;
        private readonly AncestralVaultDbContext dbContext;
        private readonly ILogger logger;

        private readonly CancellationToken token = CancellationToken.None;

        public PersonaRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            // Set up the mini-container
            container = DatabaseTestHelpers.CreateContainer(testOutputHelper);

            personaRepo = container.GetRequiredService<IPersonaRepository>();
            dbContext = container.GetRequiredService<AncestralVaultDbContext>();
            logger = container.GetRequiredService<ILogger<PersonaRepositoryTests>>();
        }


        [Fact]
        public async Task CanLoadSimpleTombstone()
        {
            // Arrange
            await DatabaseTestHelpers.PopulateDatabaseAsync(container, "test-tombstone.jsonc", logger, token);

            // Act
            var viewModel = await personaRepo.GetPersonaDetailsAsync(dbContext, "t1:p1", token);

            // Assert
            dbContext.Events.Should().HaveCount(2);
            dbContext.EventRoles.Should().HaveCount(2);

            viewModel.Should().NotBeNull();
            viewModel.Name.Should().Be("Walter Smith");

            viewModel.EventBoxItems.Should().HaveCount(2);
            viewModel.EventBoxItems.Should().Contain(x => x.EventTypeId == "birth");
            viewModel.EventBoxItems.Should().Contain(x => x.EventTypeId == "death");
        }


        public void Dispose()
        {
            container.Dispose();
        }
    }
}
