// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// A place in the world (city, county, state, country, etc.)
    /// </summary>
    /// <remarks>
    /// This deviates from the GenTech model, as it is hierarchical. The GenTech model has a separate list
    /// of place-parts for every place, but I only want one entry for a state, even though it will appear
    /// in many places.
    /// </remarks>
    [Table("places")]
    public class Place
    {
        /// <summary>
        /// Unique key identifying a single PLACE.
        /// </summary>
        [Key]
        [Column("place_id")]
        [MaxLength(50)]
        public required string PlaceId { get; set; }

        /// <summary>
        /// The ID that identifies the type of place, such as "state" or "county".
        /// </summary>
        [Column("place_type_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(PlaceType))]
        public required string PlaceTypeId { get; set; }

        /// <summary>
        /// The name of the place.
        /// </summary>
        [Column("name")]
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// The place that contains this place. For example, the state contains the county, the country contains the state, etc.
        /// </summary>
        [Column("parent_place_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(ParentPlace))]
        public string? ParentPlaceId { get; set; }

        /// <summary>
        /// The data file from which this PLACE was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile? DataFile { get; set; }
        public PlaceType? PlaceType { get; set; }
        public Place? ParentPlace { get; set; }
    }
}
