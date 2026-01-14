// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// An EVENT is any type of happening such as a particular wedding.
    /// </summary>
    /// <remarks>
    /// See p. 54 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("events")]
    public class Event
    {
        /// <summary>
        /// Unique identifier that indicates which EVENT this is.
        /// </summary>
        [Key]
        [Column("event_id")]
        [MaxLength(50)]
        public required string EventId { get; set; }

        // TODO - add EventTypeId
        // TODO - add PlaceId
        // TODO - add EventName
        // TODO - add EventDate
    }
}
