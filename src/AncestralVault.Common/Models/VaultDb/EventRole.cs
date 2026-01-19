// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// An EVENT-ROLE represents the role one PERSONA plays in an EVENT.
    /// </summary>
    [Table("event_roles")]
    public class EventRole
    {
        /// <summary>
        /// A unique key identifying this EVENT-ROLE.
        /// </summary>
        [Key]
        [Column("event_role_key")]
        public long EventRoleKey { get; set; }

        /// <summary>
        /// The ID of the persona playing this role in the event.
        /// </summary>
        [Column("persona_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(Persona))]
        public required string PersonaId { get; set; }

        /// <summary>
        /// The Key of the event in which the persona is involved.
        /// </summary>
        [Column("event_key")]
        [ForeignKey(nameof(Event))]
        public long EventKey { get; set; }

        /// <summary>
        /// The ID of the role the persona plays in the event.
        /// </summary>
        [Column("event_role_type_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(EventRoleType))]
        public required string EventRoleTypeId { get; set; }

        /// <summary>
        /// The data file from which this EVENT-ROLE was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile DataFile { get; set; } = null!;
        public Persona Persona { get; set; } = null!;
        public Event Event { get; set; } = null!;
        public EventRoleType EventRoleType { get; set; } = null!;
    }
}
