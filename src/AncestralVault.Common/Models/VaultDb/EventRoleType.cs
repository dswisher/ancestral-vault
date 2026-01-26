// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// An EVENT-ROLE-TYPE describes the role an individual can have in an EVENT (e.g., decedent, spouse, child).
    /// </summary>
    [Table("event_role_types")]
    public class EventRoleType
    {
        /// <summary>
        /// Unique key identifying a single EVENT-ROLE-TYPE.
        /// </summary>
        [Key]
        [Column("event_role_type_id")]
        [MaxLength(50)]
        public required string EventRoleTypeId { get; set; }

        /// <summary>
        /// The name of this EVENT-ROLE-TYPE, such as "Spouse", "Son", "Father", "Wife", etc.
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        // ----------------- Navigation Properties -----------------

        public ICollection<EventRole> EventRoles { get; set; } = null!;
    }
}
