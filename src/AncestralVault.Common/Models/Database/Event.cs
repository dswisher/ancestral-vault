using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.Database
{
    /// <summary>
    /// An EVENT is any type of happening such as a particular wedding.
    /// </summary>
    /// <remarks>
    /// See p. 54 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("event")]
    public class Event
    {
        /// <summary>
        /// Unique identifier that indicates which EVENT this is.
        /// </summary>
        [Key]
        [Column("event_id")]
        [MaxLength(50)]
        [JsonPropertyName("id")]
        public required string EventId { get; set; }

        // TODO - add EventTypeId
        // TODO - add PlaceId
        // TODO - add EventName
        // TODO - add EventDate
    }
}
