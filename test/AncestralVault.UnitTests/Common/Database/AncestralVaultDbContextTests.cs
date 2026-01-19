// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AncestralVault.UnitTests.Common.Database
{
    public class AncestralVaultDbContextTests
    {
        [Fact]
        public void CanCreateInMemoryDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AncestralVaultDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            // Act
            using var context = new AncestralVaultDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Assert
            context.Database.CanConnect().Should().BeTrue();
            context.Personas.Should().NotBeNull();
            context.Places.Should().NotBeNull();
            context.DataFiles.Should().NotBeNull();
            context.Events.Should().NotBeNull();
            context.EventRoleTypes.Should().NotBeNull();
            context.EventTypes.Should().NotBeNull();
            context.PlaceTypes.Should().NotBeNull();
        }
    }
}
