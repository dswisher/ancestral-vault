// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// An EVENT is something that happened to one or more individuals, in a specific place, at a specific time (though
    /// the place and/or time may not be known). The affected individuals are tied to the role via EVENT-ROLE entries.
    /// </summary>
    [Table("events")]
    public class Event
    {
        /// <summary>
        /// A unique key identifying this EVENT.
        /// </summary>
        [Key]
        [Column("event_key")]
        public long EventKey { get; set; }

        /// <summary>
        /// The ID that identifies the type of EVENT, such as "birth" or "marriage".
        /// </summary>
        [Column("event_type_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(EventType))]
        public required string EventTypeId { get; set; }

        /// <summary>
        /// The date this event occurred, if known.
        /// </summary>
        [Column("event_date")]
        [MaxLength(50)]
        public string? EventDate { get; set; }      // TODO: switch this to a "genealogical date" or some such

        /// <summary>
        /// The data file from which this EVENT was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile DataFile { get; set; } = null!;
        public EventType EventType { get; set; } = null!;

        public ICollection<EventRole> EventRoles { get; set; } = null!;
    }
}
