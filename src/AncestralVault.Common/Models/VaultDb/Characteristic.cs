// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    // TODO - do we keep this, or replace it? It seems like names should be their own thing, and not a characteristic.

    /// <summary>
    /// A CHARACTERISTIC is any data that distinguishes one person from another, such as an
    /// occupation, hair color, religion, name, and so forth. Most CHARACTERISTIC data consists
    /// of a single part value, but some data can be more complex and require the sequencing of
    /// many parts such as a person’s name.
    /// </summary>
    [Table("characteristics")]
    public class Characteristic
    {
        /// <summary>
        /// Unique identifier that indicates which CHARACTERISTIC this is.
        /// </summary>
        [Key]
        [Column("characteristic_id")]
        [MaxLength(50)]
        public required string CharacteristicId { get; set; }
    }
}
