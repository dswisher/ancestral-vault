
using GraphQL.Types;


namespace AncestralVault.Api.Schemas
{
    public class GenTechSchema : Schema
    {
        public GenTechSchema(IGenTech genTech)
        {
            Query = new GenTechQuery(genTech);
            // Mutation = new GenTechMutation(genTech);
            // Subscription = new GenTechSubscriptions(genTech);
        }
    }
}

