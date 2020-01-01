
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace AncestralVault.Models.Abstracts
{
    public class Event
    {
        public string Name { get; set; }
        public string EventType { get; set; }
        public string EventDate { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string MaritalStatus { get; set; }
        public string Race { get; set; }
        public string Role { get; set; }
        public string BirthPlace { get; set; }
        public string FatherBirthPlace { get; set; }
        public string MotherBirthPlace { get; set; }
        // public string SheetLetter { get; set; }
        // public string SheetNumber { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}

