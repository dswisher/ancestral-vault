
using System.Collections.Generic;

using AncestralVault.Models.InMemory;

namespace AncestralVault.Api.Schemas
{
    // TODO - move this to a shared lib, and rename to something like GenTechGraphProvider
    public class GenTech : IGenTech
    {
        private readonly List<Place> places = new List<Place>();

        public List<Place> AllPlaces { get { return places; } }

        public GenTech()
        {
            places.Add(new Place { Name = "Mahaska, IA" });
            places.Add(new Place { Name = "Richland Twp, Mahaska, IA" });
        }
    }
}

