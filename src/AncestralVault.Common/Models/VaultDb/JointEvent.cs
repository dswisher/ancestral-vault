// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// A JOINT-EVENT is something that happened to two individuals, in a specific place, at a specific time (though
    /// the place and/or time may not be known). The affected individuals, or principals, are personas that are
    /// tied to the event with roles, such as "bridge", "groom", etc.
    /// </summary>
    /// <remarks>
    /// There may be additional personas associated with the JOINT-EVENT as witnesses, informants, etc.
    /// </remarks>
    [Table("joint_events")]
    public class JointEvent
    {
        /// <summary>
        /// A unique key identifying this JOINT-EVENT.
        /// </summary>
        [Key]
        [Column("joint_event_key")]
        public long JointEventKey { get; set; }


        // TODO - add PlaceId
        // TODO - add EventTypeId
        // TODO - add EventName
        // TODO - add EventDate
        // TODO - link to principal Personas, with their roles
        // TODO - link to additional Personas (witnesses, informants, etc.)
    }
}
