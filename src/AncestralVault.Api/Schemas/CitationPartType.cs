
using GraphQL.Types;

using AncestralVault.Models.InMemory;

namespace AncestralVault.Api.Schemas
{
    public class CitationPartType : ObjectGraphType<CitationPart>
    {
        public CitationPartType()
        {
            // TODO - should we just include a link to the part-type?
            Field(x => x.Type.Name).Description("The name of the part.");
            Field(x => x.Value).Description("The value of the part.");
        }
    }
}

