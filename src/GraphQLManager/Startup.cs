using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQLManager.GraphQLOperation;
using GraphQLParser;
using Grpc.Core;
using Grpc.Net.Client;
using IdentityModel.Client;
using MemberManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GraphQLManager
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        private IConfiguration _config { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<SportAdminSchema>();

            services.AddGraphQL( options =>
            {
                options.EnableMetrics = false;
            })
                .AddGraphQLAuthorization()
                .AddDataLoader()
                .AddSystemTextJson()
                .AddGraphTypes(ServiceLifetime.Singleton)
                .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User, Token = httpContext.Request.Headers["Authorization"] });

            services.AddAuthentication("bearer")
                   .AddIdentityServerAuthentication("bearer", options =>
                   {
                       options.Authority = _config["IdentityManager:ServerUrl"];
                       options.RequireHttpsMetadata = _config.GetValue<bool>("IdentityManager:RequireHttpsMetadata");
                    });
            
            services
                .AddGrpcClient<Members.MembersClient>(o =>
                {
                    o.Address = new Uri(_config["MemberManager:ServerUrl"]);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var cert = new X509Certificate2(_config["MemberManager:CertFileName"],
                                                    _config["MemberManager:CertPassword"]);

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseGraphQL<SportAdminSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
