
using GraphQL.Types;

using AncestralVault.Models.InMemory;

namespace AncestralVault.Api.Schemas
{
    public class PlaceType : ObjectGraphType<Place>
    {
        public PlaceType()
        {
            Field(x => x.Name).Description("The name of the place.");
        }
    }
}

