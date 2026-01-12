using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.Database
{
    /// <summary>
    /// A CHARACTERISTIC is any data that distinguishes one person from another, such as an
    /// occupation, hair color, religion, name, and so forth. Most CHARACTERISTIC data consists
    /// of a single part value, but some data can be more complex and require the sequencing of
    /// many parts such as a person’s name.
    /// </summary>
    /// <remarks>
    /// See p. 49 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("characteristic")]
    public class Characteristic
    {
        /// <summary>
        /// Unique identifier that indicates which CHARACTERISTIC this is.
        /// </summary>
        [Key]
        [Column("characteristic_id")]
        [MaxLength(50)]
        [JsonPropertyName("id")]
        public required string CharacteristicId { get; set; }
    }
}
