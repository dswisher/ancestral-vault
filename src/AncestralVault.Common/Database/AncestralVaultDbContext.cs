// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.VaultDb;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Database
{
    public class AncestralVaultDbContext : DbContext
    {
        public AncestralVaultDbContext(DbContextOptions<AncestralVaultDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<CompositePersona> CompositePersonas { get; init; }
        public virtual DbSet<DataFile> DataFiles { get; init; }
        public virtual DbSet<Event> Events { get; init; }
        public virtual DbSet<EventRole> EventRoles { get; init; }
        public virtual DbSet<EventRoleType> EventRoleTypes { get; init; }
        public virtual DbSet<EventType> EventTypes { get; init; }
        public virtual DbSet<Persona> Personas { get; init; }
        public virtual DbSet<PersonaAssertion> PersonaAssertions { get; init; }
        public virtual DbSet<Place> Places { get; init; }
        public virtual DbSet<PlaceType> PlaceTypes { get; init; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e.EventDate)
                .HasConversion<GenealogicalDateConverter>()
                .HasMaxLength(50);
        }
    }
}
