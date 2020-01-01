
using System.Collections.Generic;

namespace AncestralVault.Models.InMemory
{
    public interface IMemoryRepository
    {
        List<Place> AllPlaces { get; }
        List<Source> AllSources { get; }

        Source CreateSource();
    }


    public class MemoryRepository : IMemoryRepository
    {
        private readonly List<Place> places = new List<Place>();
        private readonly List<Source> sources = new List<Source>();

        public List<Place> AllPlaces { get { return places; } }
        public List<Source> AllSources { get { return sources; } }


        public MemoryRepository()
        {
            // TODO - hacky hard-coded data!
            places.Add(new Place { Name = "Mahaska, IA" });
            places.Add(new Place { Name = "Richland Twp, Mahaska, IA" });
        }


        public Source CreateSource()
        {
            var source = new InMemory.Source
            {
                CitationParts = new List<CitationPart>()
            };

            sources.Add(source);

            return source;
        }
    }
}

