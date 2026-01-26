// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// An EVENT-TYPE describes the type of an EVENT (e.g., birth, death, marriage).
    /// </summary>
    [Table("event_types")]
    public class EventType
    {
        /// <summary>
        /// Unique key identifying a single EVENT-TYPE.
        /// </summary>
        [Key]
        [Column("event_type_id")]
        [MaxLength(50)]
        public required string EventTypeId { get; set; }

        /// <summary>
        /// The name of this EVENT-TYPE, such as "Birth", "Death", "Marriage", "Residence", etc.
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }


        // ----------------- Navigation Properties -----------------

        public ICollection<Event> Events { get; set; } = null!;
    }
}
