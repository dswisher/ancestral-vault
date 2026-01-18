// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// An assertion that a given PERSONA belongs (or does not belong) to a given COMPOSITE-PERSONA.
    /// </summary>
    [Table("persona_assertions")]
    public class PersonaAssertion
    {
        /// <summary>
        /// A unique key identifying this PERSONA-ASSERTION.
        /// </summary>
        [Key]
        [Column("composite_persona_assertion_key")]
        public long CompositePersonaAssertionKey { get; set; }

        /// <summary>
        /// The COMPOSITE-PERSONA to which this assertion applies.
        /// </summary>
        [Column("composite_persona_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(CompositePersona))]
        public required string CompositePersonaId { get; set; }

        /// <summary>
        /// The PERSONA to which this assertion applies.
        /// </summary>
        [Column("persona_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Persona))]
        public required string PersonaId { get; set; }

        /// <summary>
        /// The reason why the researcher believes that the PERSONA belongs (or does not belong) to the COMPOSITE-PERSONA.
        /// </summary>
        [Column("rationale")]
        [MaxLength(1024)]
        public string? Rationale { get; set; }

        /// <summary>
        /// If true, the PERSONA is believed NOT to belong to the COMPOSITE-PERSONA, and the rationale explains why.
        /// </summary>
        [Column("negated")]
        public bool? Negated { get; set; }

        /// <summary>
        /// The data file from which this PERSONA-ASSERTION was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile? DataFile { get; set; }

        public CompositePersona? CompositePersona { get; set; }
        public Persona? Persona { get; set; }
    }
}
