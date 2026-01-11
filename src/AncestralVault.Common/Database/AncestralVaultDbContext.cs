using AncestralVault.Common.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Database
{
    public class AncestralVaultDbContext : DbContext
    {
        public AncestralVaultDbContext(DbContextOptions<AncestralVaultDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<DataFile> DataFiles { get; init; }
        public virtual DbSet<Representation> Representations { get; init; }
        public virtual DbSet<RepresentationType> RepresentationTypes { get; init; }
    }
}
