// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// A COMPOSITE-PERSONA is an aggregation of multiple PERSONA records that are believed to
    /// represent the same individual.
    /// </summary>
    [Table("composite_personas")]
    public class CompositePersona
    {
        /// <summary>
        /// Unique key identifying this COMPOSITE-PERSONA.
        /// </summary>
        [Key]
        [Column("composite_persona_id")]
        [MaxLength(50)]
        public required string CompositePersonaId { get; set; }

        /// <summary>
        /// The name by which the researcher refers to this COMPOSITE-PERSONA.
        /// </summary>
        [Required]
        [Column("name")]
        [MaxLength(100)]
        public required string Name { get; set; }

        /// <summary>
        /// The data file from which this COMPOSITE-PERSONA was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile DataFile { get; set; } = null!;
    }
}
