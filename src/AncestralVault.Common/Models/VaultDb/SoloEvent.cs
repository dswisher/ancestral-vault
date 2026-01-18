// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// A SOLO-EVENT is something that happened to one individual, in a specific place, at a specific time (though
    /// the place and/or time may not be known). The affected individual, or principal, is a persona that is
    /// tied to the event with a role, such as "resident", "decedent", etc.
    /// </summary>
    /// <remarks>
    /// There may be additional personas associated with the SOLO-EVENT as witnesses, informants, etc.
    /// </remarks>
    [Table("solo_events")]
    public class SoloEvent
    {
        /// <summary>
        /// A unique key identifying this SOLO-EVENT.
        /// </summary>
        [Key]
        [Column("solo_event_key")]
        public long SoloEventKey { get; set; }

        /// <summary>
        /// The type of this event.
        /// </summary>
        [Column("event_type")]
        [MaxLength(50)]
        public required string EventType { get; set; }      // TODO - this should be an enum or FK

        /// <summary>
        /// The principal persona associated with this event.
        /// </summary>
        [Column("principal_persona_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(PrincipalPersona))]
        public required string PrincipalPersonaId { get; set; }

        /// <summary>
        /// The role the principal persona had in this event.
        /// </summary>
        [Column("principal_role")]
        [MaxLength(50)]
        public required string PrincipalRole { get; set; }

        // TODO - add PlaceId

        /// <summary>
        /// The date this event occurred, if known.
        /// </summary>
        [Column("event_date")]
        [MaxLength(50)]
        public string? EventDate { get; set; }      // TODO: switch this to a "genealogical date" or some such

        // TODO - link to additional Personas (witnesses, informants, etc.)

        /// <summary>
        /// The data file from which this PERSONA was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile DataFile { get; set; } = null!;
        public Persona PrincipalPersona { get; set; } = null!;
    }
}
