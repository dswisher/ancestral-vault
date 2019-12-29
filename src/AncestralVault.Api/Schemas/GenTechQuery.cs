
using GraphQL.Types;


namespace AncestralVault.Api.Schemas
{
    public class GenTechQuery : ObjectGraphType
    {
        public GenTechQuery(IGenTech genTech)
        {
            Field<ListGraphType<PlaceType>>("places", resolve: context => genTech.AllPlaces);
        }
    }
}

