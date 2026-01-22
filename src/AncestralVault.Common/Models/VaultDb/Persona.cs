// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
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
        /// Unique key identifying this PERSONA.
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
        /// The prefix, such as "Dr.". Also known as title or prenomial.
        /// </summary>
        [Column("name_prefix")]
        [MaxLength(20)]
        public string? NamePrefix { get; set; }

        /// <summary>
        /// The given name, such as "John David", "Robert", or "Mary Jane". Also known as first name, personal name, or mononame.
        /// </summary>
        /// <remarks>
        /// This may include a middle name, middle initial, or multiple given names.
        /// </remarks>
        [Column("given_name")]
        [MaxLength(50)]
        public string? GivenNames { get; set; }

        /// <summary>
        /// The surname, such as "Smith" or "Johnson". Also known as last name or family name.
        /// </summary>
        [Column("surname")]
        [Required]
        [MaxLength(50)]
        public required string Surname { get; set; }

        /// <summary>
        /// The suffix, such as "Jr". Also known as postnomial.
        /// </summary>
        [Column("name_suffix")]
        [MaxLength(20)]
        public string? NameSuffix { get; set; }

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

        public DataFile DataFile { get; set; } = null!;

        public ICollection<EventRole> EventRoles { get; set; } = null!;
    }
}
