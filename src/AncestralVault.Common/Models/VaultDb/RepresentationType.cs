// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    // TODO - do we keep this, or replace it?

    /// <summary>
    /// Contains a list of the types of representations of evidence, such as text, a TIF bitmap, a GIF bitmap, a WAV file, or other forms.
    /// </summary>
    /// <remarks>
    /// See p. 68 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("representation_types")]
    public class RepresentationType
    {
        /// <summary>
        /// A unique key that identifies this REPRESENTATION-TYPE.
        /// </summary>
        [Key]
        [Column("representation_type_id")]
        [MaxLength(50)]
        public required string RepresentationTypeId { get; set; }

        /// <summary>
        /// The name, such as "Text", "PNG Image", etc.
        /// </summary>
        [Required]
        [Column("name")]
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// The data file from which this REPRESENTATION-TYPE was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile? DataFile { get; set; }
    }
}
