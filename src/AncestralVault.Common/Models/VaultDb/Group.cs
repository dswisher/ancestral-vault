// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncestralVault.Common.Models.VaultDb
{
    /// <summary>
    /// In genealogical data, there are group members for which we can’t identify query
    /// conditions to return the set. In other words, membership in a group such as “men
    /// who worked on the Davison Road in August, 1851” may be important genealogically,
    /// but no other attributes will sufficiently code for this. Thus, those members need
    /// to be tagged as explicit members of one or more groups. Groups are also used in
    /// this data model for concepts such as a group of children for a union of a man and woman.
    /// </summary>
    /// <remarks>
    /// See p. 57 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("groups")]
    public class Group
    {
        /// <summary>
        /// Unique identifier that indicates which GROUP this is.
        /// </summary>
        [Key]
        [Column("group_id")]
        [MaxLength(50)]
        public required string GroupId { get; set; }

        // TODO - add GroupTypeId
        // TODO - add PlaceId
        // TODO - add GroupName
        // TODO - add GroupDate
        // TODO - add GroupCriteria
    }
}
