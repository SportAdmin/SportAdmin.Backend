using GraphQL;
using GraphQL.Types;
using GraphQLManager.GraphQLOperation.Type.Member;
using GraphQLManager.Interface;
using GraphQLManager.Services;
using MemberManager;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace GraphQLManager.GraphQLOperation
{
    public class SportAdminQuery : ObjectGraphType
    {
        public SportAdminQuery(IMemberService memberService)
        {
            ApolloExampel();

            FieldAsync<MemberGraphType>(
                "me",
                resolve: async context =>
                {
                    var UserContext = context.UserContext as IProvideClaimsPrincipal;

                    if (!UserContext.User.Identity.IsAuthenticated)
                    {
                        return null;
                    }

                    string id = UserContext.User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value;

                    return await memberService.GetMemberAsync(id);
                }
            );
        }

        private void ApolloExampel()
        {
            Field<StringGraphType>(
                "hello",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "name" }
                  ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    return $"Hello {name}";
                }
            );
        }
    }
}
