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
        private readonly IConfiguration _config;

        public SportAdminQuery(IConfiguration config)
        {
            _config = config;

            FieldAsync<MemberGraphType>(
                "me",
                resolve: async context =>
                {
                    try
                    {
                        var UserContext = context.UserContext as IProvideClaimsPrincipal;

                        var cert = new X509Certificate2(_config["MemberManager:CertFileName"],
                                                        _config["MemberManager:CertPassword"]);

                        var handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(cert);

                        var opt = new GrpcChannelOptions()
                        {
                            HttpClient = new HttpClient(handler)
                        };

                        using var channel = GrpcChannel.ForAddress(_config["MemberManager:ServerUrl"], opt);
                        var client = new Members.MembersClient(channel);

                        var headers = new Metadata();
                        headers.Add("Authorization", UserContext.Token);

                        var reply = await client.getMemberAsync(
                                      new MemberRequest { Id = UserContext.User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value },
                                      headers);

                        MemberItem item = new MemberItem()
                        {
                            Id = UserContext.User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value,
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
