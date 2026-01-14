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


        public virtual DbSet<Assertion> Assertions { get; init; }
        public virtual DbSet<Characteristic> Characteristics { get; init; }
        public virtual DbSet<DataFile> DataFiles { get; init; }
        public virtual DbSet<Event> Events { get; init; }
        public virtual DbSet<Group> Groups { get; init; }
        public virtual DbSet<Persona> Personas { get; init; }
        public virtual DbSet<Representation> Representations { get; init; }
        public virtual DbSet<RepresentationType> RepresentationTypes { get; init; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assertion>(entity =>
            {
                // Make the FKs optional and configure relationships
                // entity.HasOne(a => a.Subject1Persona)
                //     .WithMany()
                //     .HasForeignKey(a => a.Subject1PersonaId)
                //     .OnDelete(DeleteBehavior.Restrict);

                // Add a check constraint to ensure only one FK is set
                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Assertion_Subject1_OneType",
                    "(CASE WHEN subject1_persona_id IS NOT NULL THEN 1 ELSE 0 END + CASE WHEN subject1_event_id IS NOT NULL THEN 1 ELSE 0 END + CASE WHEN subject1_characteristic_id IS NOT NULL THEN 1 ELSE 0 END + CASE WHEN subject1_group_id IS NOT NULL THEN 1 ELSE 0 END) = 1"));
            });
        }
    }
}
