
using System.Collections.Generic;

using AncestralVault.Models.InMemory;

namespace AncestralVault.Api.Schemas
{
    // TODO - move this to a shared lib, and rename to something like IGenTechGraphProvider
    public interface IGenTech
    {
        List<Place> AllPlaces { get; }
    }
}
