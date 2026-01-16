// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// An EVENT is something that happened to one or more personas, in a specific place, at a specific time (though
    /// the place and/or time may not be known). Each persona is tied to an event with a role, such as "child",
    /// "groom", "resident", "witness", etc.
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


        // TODO - add EventTypeId
        // TODO - add PlaceId
        // TODO - add EventName
        // TODO - add EventDate
    }
}
