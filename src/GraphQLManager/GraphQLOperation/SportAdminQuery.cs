using GraphQL;
using GraphQL.Types;
using GraphQLManager.GraphQLOperation.Type.Member;
using System.Linq;
using MemberManager;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Grpc.Core;
using GraphQL.Validation;

namespace GraphQLManager.GraphQLOperation
{
    public class SportAdminQuery : ObjectGraphType
    {
        public SportAdminQuery(IConfiguration config, Members.MembersClient client)
        {
            FieldAsync<MemberGraphType>(
                "me",
                resolve: async context =>
                {
                    try
                    {
                        var UserContext = context.UserContext as IProvideClaimsPrincipal;

                        if(!UserContext.User.Identity.IsAuthenticated)
                        {
                            return null;
                        }

                        var reply = await client.getMemberAsync(
                                      new MemberRequest { Id = UserContext.User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value });

                        MemberItem item = new MemberItem()
                        {
                            Id = reply.Id,
                            FirstName = reply.FirstName,
                            LastName = reply.LastName
                        };

                        return item;
                    }
                    catch (System.Exception ex)
                    {   
                        throw;
                    }
                }
            );
        }
    }
}
