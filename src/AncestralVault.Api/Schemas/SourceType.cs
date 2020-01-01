
using GraphQL.Types;

using AncestralVault.Models.InMemory;

namespace AncestralVault.Api.Schemas
{
    public class SourceType : ObjectGraphType<Source>
    {
        public SourceType()
        {
            Field(x => x.Comments).Description("Comments about this source.");

            // Field(x => x.CitationParts).Description("Citation parts.");
            Field<ListGraphType<CitationPartType>>("citationParts", "The parts of the citation.");
        }
    }
}

