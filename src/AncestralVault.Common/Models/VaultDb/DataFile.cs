// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// A data file from which data has been ingested into the system.
    /// </summary>
    /// <remarks>
    /// When a file is updated, this is used to replace the existing data.
    /// </remarks>
    [Table("data_files")]
    public class DataFile
    {
        /// <summary>
        /// A unique key identifying this data file.
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
