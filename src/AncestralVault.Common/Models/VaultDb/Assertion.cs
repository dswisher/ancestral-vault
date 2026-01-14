// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// Contains the lowest level raw conclusional data in a special atomic form. This involves
    /// an interpretation by the researcher ranging from trivial to complex. This entity also
    /// contains higher level conclusional data from lower level assertions, so that all assertions
    /// can be tracked through layers of reasoning back to their original evidential statement
    /// forms. Assertions should not be deleted, but an attribute (Disproved) exists to nullify
    /// erroneous conclusions so that the erroneous reasoning can be preserved and marked as believed
    /// to be no longer valid. Everyone’s work has value, even if it is later proved to be wrong.
    /// Since all assertions are tagged according to their origin, it is possible to store other’s
    /// assertions as well and identify that data as such. While most assertions are tied to particular
    /// SOURCE excerpts (the Content attribute in REPRESENTATION) or previous assertions, an assertion
    /// can apply to an entire SOURCE.
    /// </summary>
    /// <remarks>
    /// See p. 46 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("assertions")]
    public class Assertion
    {
        /// <summary>
        /// Unique identifier that indicates which EVENT this is.
        /// </summary>
        [Key]
        [Column("assertion_id")]
        [MaxLength(50)]
        public required string AssertionId { get; set; }

        // TODO - add SuretySchemePartId
        // TODO - add ResearcherId
        // TODO - add SourceId

#if false
        /// <summary>
        /// Can be either PERSONA, EVENT, CHARACTERISTIC, or GROUP.
        /// </summary>
        [Column("subject1_type")]
        [MaxLength(50)]
        [JsonPropertyName("subject1-type")]
        public required string Subject1Type { get; set; }

        /// <summary>
        /// A pointer to the appropriate PERSONA, EVENT, CHARACTERISTIC, or GROUP attribute of ID.
        /// </summary>
        [Column("subject1_id")]
        [MaxLength(50)]
        [JsonPropertyName("subject1-id")]
        public required string Subject1Id { get; set; }
#endif

        [Column("subject1_persona_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Persona1))]
        public string? Subject1PersonaId { get; set; }

        [Column("subject1_event_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Event1))]
        public string? Subject1EventId { get; set; }

        [Column("subject1_characteristic_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Characteristic1))]
        public string? Subject1CharacteristicId { get; set; }

        [Column("subject1_group_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Group1))]
        public string? Subject1GroupId { get; set; }


#if false
        /// <summary>
        /// Can be either PERSONA, EVENT, CHARACTERISTIC, or GROUP.
        /// </summary>
        [Column("subject2_type")]
        [MaxLength(50)]
        [JsonPropertyName("subject2-type")]
        public required string Subject2Type { get; set; }

        /// <summary>
        /// A pointer to the appropriate PERSONA, EVENT, CHARACTERISTIC, or GROUP attribute of ID.
        /// </summary>
        [Column("subject2_id")]
        [MaxLength(50)]
        [JsonPropertyName("subject2-id")]
        public required string Subject2Id { get; set; }
#endif

        [Column("subject2_persona_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Persona2))]
        public string? Subject2PersonaId { get; set; }

        [Column("subject2_event_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Event2))]
        public string? Subject2EventId { get; set; }

        [Column("subject2_characteristic_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Characteristic2))]
        public string? Subject2CharacteristicId { get; set; }

        [Column("subject2_group_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Group2))]
        public string? Subject2GroupId { get; set; }

        // TODO - add Value
        // TODO - add Rationale
        // TODO - add Disproved

        /// <summary>
        /// The data file from which this ASSERTION was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        // Subject 1
        public Persona? Persona1 { get; set; }
        public Event? Event1 { get; set; }
        public Characteristic? Characteristic1 { get; set; }
        public Group? Group1 { get; set; }

        // Subject 2
        public Persona? Persona2 { get; set; }
        public Event? Event2 { get; set; }
        public Characteristic? Characteristic2 { get; set; }
        public Group? Group2 { get; set; }

        public DataFile? DataFile { get; set; }
    }
}
