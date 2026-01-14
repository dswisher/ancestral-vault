// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// Contains the representation of a Source in a variety of multimedia formats as needed, including old
    /// fashioned text, plus it contains a pointer to a physical file if the representation cannot be stored
    /// within the data model.
    /// </summary>
    /// <remarks>
    /// See p. 67 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("representations")]
    public class Representation
    {
        /// <summary>
        /// The key of this record.
        /// </summary>
        /// <remarks>
        /// This is not part of the GenTech model, but is needed by Entity Framework.
        /// </remarks>
        [Key]
        [Column("representation_key")]
        public long RepresentationKey { get; set; }

        // TODO - add SourceId

        /// <summary>
        /// Link to the RepresentationType table.
        /// </summary>
        [Required]
        [Column("representation_type_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(RepresentationType))]
        public required string RepresentationTypeId { get; set; }

        /// <summary>
        /// If the REPRESENTATION is external to the data model, such as a stored photograph that is not scanned
        /// into a computer system, this code tells the researcher where the REPRESENTATION is physically filed
        /// or stored.
        /// </summary>
        /// <remarks>
        /// This may be a relative file path, for images stored on disk.
        /// </remarks>
        [Column("physical_file_code")]
        [MaxLength(255)]
        public string? PhysicalFileCode { get; set; }

        /// <summary>
        /// Often the SOURCE medium is paper, but it can be electronic, stone in the case of a tombstone,
        /// or other exotic media.
        /// </summary>
        [Column("medium")]
        [MaxLength(50)]
        public string? Medium { get; set; }

        // TODO - add Content

        /// <summary>
        /// Any comments that are required to describe this REPRESENTATION.
        /// </summary>
        [Column("comments")]
        [MaxLength(1024)]
        public string? Comments { get; set; }

        /// <summary>
        /// The data file from which this REPRESENTATION was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile? DataFile { get; set; }
        public RepresentationType? RepresentationType { get; set; }
    }
}
