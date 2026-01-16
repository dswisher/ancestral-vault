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


        public virtual DbSet<Characteristic> Characteristics { get; init; }
        public virtual DbSet<DataFile> DataFiles { get; init; }
        public virtual DbSet<JointEvent> JointEvents { get; init; }
        public virtual DbSet<Persona> Personas { get; init; }
        public virtual DbSet<Place> Places { get; init; }
        public virtual DbSet<PlaceType> PlaceTypes { get; init; }
        public virtual DbSet<Representation> Representations { get; init; }
        public virtual DbSet<RepresentationType> RepresentationTypes { get; init; }
        public virtual DbSet<SoloEvent> SoloEvents { get; init; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
