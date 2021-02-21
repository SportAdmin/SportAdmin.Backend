using GraphQL.Types;
using GraphQLManager.GraphQLOperation.Type.Apollo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLManager.GraphQLOperation
{
    public class SoccerClubMutation : ObjectGraphType
    {
        public SoccerClubMutation()
        {
            Field<UploadGraphType>(
                 "singleUpload",
                 arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "file" }
                ),
                resolve: context =>
                {
                    return new UploadItem() { Id = "2", Filename = "test.se", Mimetype = "image/jpg", Path = "tert", Encoding = "json" };
                }
            );
        }

    }
}
