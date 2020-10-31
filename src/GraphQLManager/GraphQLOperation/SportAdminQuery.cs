using GraphQL;
using GraphQL.Types;
using GraphQLManager.GraphQLOperation.Type.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQLManager.GraphQLOperation
{
    public class SportAdminQuery : ObjectGraphType
    {
        public SportAdminQuery()
        {
            Field<MemberGraphType>(
                "member",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id", }),
                resolve: context =>
                {

                    var UserContext = context.UserContext as IProvideClaimsPrincipal;

                    var id = context.GetArgument<string>("id");

                    MemberItem item = new MemberItem() { Id = UserContext.User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value };

                    return item;
                }
            ); 
        }
    }
}
