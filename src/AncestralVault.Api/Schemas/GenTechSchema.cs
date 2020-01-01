
using GraphQL.Types;

using AncestralVault.Models.InMemory;


namespace AncestralVault.Api.Schemas
{
    public class GenTechSchema : Schema
    {
        public GenTechSchema(IMemoryRepository repo)
        {
            Query = new GenTechQuery(repo);
            // Mutation = new GenTechMutation(genTech);
            // Subscription = new GenTechSubscriptions(genTech);
        }
    }
}

