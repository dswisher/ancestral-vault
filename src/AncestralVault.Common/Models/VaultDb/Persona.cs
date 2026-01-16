// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// A PERSONA is a single individual that appears in a piece of evidence.
    /// </summary>
    [Table("personas")]
    public class Persona
    {
        /// <summary>
        /// Unique key identifying a single PERSONA.
        /// </summary>
        [Key]
        [Column("persona_id")]
        [MaxLength(50)]
        public required string PersonaId { get; set; }

        /// <summary>
        /// The name, as it appears in the evidence.
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Free-form notes about this PERSONA.
        /// </summary>
        [Column("notes")]
        [MaxLength(1024)]
        public string? Notes { get; set; }

        /// <summary>
        /// The data file from which this PERSONA was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile? DataFile { get; set; }
    }
}
