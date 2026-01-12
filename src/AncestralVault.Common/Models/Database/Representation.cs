using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AncestralVault.Common.Models.Database
{
    /// <summary>
    /// Contains the representation of a Source in a variety of multimedia formats as needed, including old
    /// fashioned text, plus it contains a pointer to a physical file if the representation cannot be stored
    /// within the data model.
    /// </summary>
    /// <remarks>
    /// See p. 67 in the GenTech Data Model, v1.1.
    /// </remarks>
    [Table("representation")]
    public class Representation
    {
        /// <summary>
        /// The key of this record.
        /// </summary>
        /// <remarks>
        /// This is not part of the GenTech model, but is needed by Entity Framework.
        /// </remarks>
        [Key]
        [Column("representation_key")]
        public long RepresentationKey { get; set; }

        // TODO - add SourceId

        /// <summary>
        /// Link to the RepresentationType table.
        /// </summary>
        [Required]
        [Column("representation_type_id")]
        [MaxLength(50)]
        [ForeignKey(nameof(RepresentationType))]
        [JsonPropertyName("type")]
        public required string RepresentationTypeId { get; set; }

        /// <summary>
        /// If the REPRESENTATION is external to the data model, such as a stored photograph that is not scanned
        /// into a computer system, this code tells the researcher where the REPRESENTATION is physically filed
        /// or stored.
        /// </summary>
        /// <remarks>
        /// This may be a relative file path, for images stored on disk.
        /// </remarks>
        [Column("physical_file_code")]
        [MaxLength(255)]
        [JsonPropertyName("file-code")]
        public string? PhysicalFileCode { get; set; }

        /// <summary>
        /// Often the SOURCE medium is paper, but it can be electronic, stone in the case of a tombstone,
        /// or other exotic media.
        /// </summary>
        [Column("medium")]
        [MaxLength(50)]
        [JsonPropertyName("medium")]
        public string? Medium { get; set; }

        // TODO - add Content

        /// <summary>
        /// Any comments that are required to describe this REPRESENTATION.
        /// </summary>
        [Column("comments")]
        [MaxLength(1024)]
        [JsonPropertyName("comments")]
        public string? Comments { get; set; }

        /// <summary>
        /// The data file from which this REPRESENTATION was ingested.
        /// </summary>
        [Column("data_file_key")]
        [ForeignKey(nameof(DataFile))]
        public long DataFileKey { get; set; }


        // ----------------- Navigation Properties -----------------

        public DataFile? DataFile { get; set; }
        public RepresentationType? RepresentationType { get; set; }
    }
}
