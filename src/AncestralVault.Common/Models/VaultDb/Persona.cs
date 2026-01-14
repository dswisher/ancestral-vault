// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// Contains the core identification for each individual in genealogical data, and allows information
    /// about similarly named or identically named people to be brought together, after suitable analysis,
    /// in the same aggregate individual. Because real human beings leave data tracks through time as if
    /// they were disparate shadow personas, this entity allows the genealogical researcher to tie together
    /// data from different personas that he or she believes belong to the same real person. The mechanism
    /// for this, discussed in the text, is to make different PERSONAs part of the same GROUP.
    /// </summary>
    /// <remarks>
    /// See p. 60 in the GenTech Data Model, v1.1.
    /// </remarks>
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
        /// The entire name that this PERSONA is known by. This can be a special instance from a single
        /// record (from SOURCE and REPRESENTATION) like “John Q. Smith”, or it can be a composite name built
        /// up from many separate instances, such as “John Quincy (Butch) Smith”, that never actually appear
        /// in any record, but which reflects the name the way the RESEARCHER wishes to tag the individual.
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Any narrative necessary to distinguish this person.
        /// </summary>
        [Column("description")]
        [MaxLength(1024)]
        public string? Description { get; set; }

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
