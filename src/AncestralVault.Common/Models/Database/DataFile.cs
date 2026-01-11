using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.Database
{
    /// <summary>
    /// A data file from which data has been ingested into the system.
    /// </summary>
    /// <remarks>
    /// When a file is updated, this is used to replace the existing data.
    /// </remarks>
    [Table("data_file")]
    public class DataFile
    {
        /// <summary>
        /// The key of this entry.
        /// </summary>
        [Key]
        [Column("data_file_key")]
        public long DataFileKey { get; set; }

        /// <summary>
        /// The path of the data file, relative to the vault root.
        /// </summary>
        /// <remarks>
        /// This is always stored using forward slashes (/) as directory separators.
        /// </remarks>
        [Required]
        [Column("relative_path")]
        [MaxLength(255)]
        public required string RelativePath { get; set; }
    }
}
