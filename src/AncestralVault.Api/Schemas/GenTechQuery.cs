
using GraphQL.Types;

using AncestralVault.Models.InMemory;


namespace AncestralVault.Api.Schemas
{
    public class GenTechQuery : ObjectGraphType
    {
        public GenTechQuery(IMemoryRepository repo)
        {
            Field<ListGraphType<PlaceType>>("places", resolve: context => repo.AllPlaces);
            Field<ListGraphType<SourceType>>("sources", resolve: context => repo.AllSources);
        }
    }
}

