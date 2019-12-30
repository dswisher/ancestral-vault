
using System.Collections.Generic;


namespace AncestralVault.Models.InMemory
{
    public class Source
    {
        // TODO - SourceId
        // TODO - HigherSourceId
        // TODO - SubjectPlaceId
        // TODO - JurisdictionPlaceId
        // TODO - ResearcherId
        public string SubjectDate { get; set; }     // TODO - date
        public string Comments { get; set; }
        // TODO - list of repositories

        public List<CitationPart> CitationParts { get; set; }
    }
}

