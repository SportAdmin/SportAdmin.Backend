using Grpc.Core;
using Grpc.Net.Client;
using MemberManager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GraphQLManager.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddMemberClient(this IServiceCollection build, IConfiguration config)
        {
            return build.AddGrpcClient<Members.MembersClient>(o =>
             {
                 o.Address = new Uri(config["MemberManager:ServerUrl"]);
             })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var cert = new X509Certificate2(config["MemberManager:CertFileName"],
                                                    config["MemberManager:CertPassword"]);

                    var handler = new HttpClientHandler();
                    handler.ClientCertificates.Add(cert);

                    var opt = new GrpcChannelOptions()
                    {
                        HttpClient = new HttpClient(handler)
                    };

                    return handler;
                })
                .ConfigureChannel((s, o) =>
                {
                    var callCredentials = CallCredentials.FromInterceptor((context, metadata) =>
                    {
                        string token = s.GetService<IHttpContextAccessor>().HttpContext.Request.Headers["Authorization"];

                        if (!string.IsNullOrEmpty(token))
                        {
                            metadata.Add("Authorization", token);
                        }
                        return Task.CompletedTask;
                    });

                    o.Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials);
                });
        }
    }
}
