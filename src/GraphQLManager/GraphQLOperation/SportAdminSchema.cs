using GraphQL.Types;
using GraphQL.Utilities;
using System;

namespace GraphQLManager.GraphQLOperation
{
    public class SportAdminSchema : Schema
    {
        public SportAdminSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<SportAdminQuery>();
            Mutation = serviceProvider.GetRequiredService<SoccerClubMutation>();
            Description = "The schema for the Soccer Club";
        }
    }
}
