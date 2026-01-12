using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.Database
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
    [Table("group")]
    public class Group
    {
        /// <summary>
        /// Unique identifier that indicates which GROUP this is.
        /// </summary>
        [Key]
        [Column("group_id")]
        [MaxLength(50)]
        [JsonPropertyName("id")]
        public required string GroupId { get; set; }

        // TODO - add GroupTypeId
        // TODO - add PlaceId
        // TODO - add GroupName
        // TODO - add GroupDate
        // TODO - add GroupCriteria
    }
}
