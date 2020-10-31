using GraphQL;
using GraphQL.Types;
using GraphQLManager.GraphQLOperation.Type.Member;
using System.Linq;
using MemberManager;
using Grpc.Net.Client;

namespace GraphQLManager.GraphQLOperation
{
    public class SportAdminQuery : ObjectGraphType
    {
        public SportAdminQuery()
        {
            FieldAsync<MemberGraphType>(
                "me",
                resolve: async context =>
                {
                    var UserContext = context.UserContext as IProvideClaimsPrincipal;

                    // The port number(5001) must match the port of the gRPC server.
                    using var channel = GrpcChannel.ForAddress("https://localhost:5005");
                    var client = new Members.MembersClient(channel);
                    var reply = await client.getMemberAsync(
                                      new MemberRequest { Id = UserContext.User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value });
                    
                    MemberItem item = new MemberItem() 
                    { 
                        Id = UserContext.User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value,
                        FirstName = reply.Firstname,
                        LastName = reply.Lastname
                    };

                    return item;
                }
            ); 
        }
    }
}
