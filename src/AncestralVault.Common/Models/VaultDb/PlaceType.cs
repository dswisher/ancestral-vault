// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// A type of place, such as "City", "County", "State", "Country", etc.
    /// </summary>
    [Table("place_types")]
    public class PlaceType
    {
        /// <summary>
        /// Unique key identifying a single PLACE-TYPE.
        /// </summary>
        [Key]
        [Column("place_type_id")]
        [MaxLength(50)]
        public required string PlaceTypeId { get; set; }

        /// <summary>
        /// The name of this PLACE-TYPE, such as "City", "County", "State", "Country", etc.
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}
