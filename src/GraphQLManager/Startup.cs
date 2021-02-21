using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQLManager.Extensions;
using GraphQLManager.GraphQLOperation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphQLManager
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        private IConfiguration _config { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                                          .AllowAnyHeader()
                                                          .AllowAnyMethod();
                                  });
            });
            services.AddHttpContextAccessor();

            services.AddSportAdminService();
            services.AddSportAdminGrpcRepository();

            services.AddScoped<SportAdminSchema>();

            services.AddGraphQL( options =>
            {
                options.EnableMetrics = false;
            })
                .AddGraphQLAuthorization()
                .AddDataLoader()
                .AddSystemTextJson()
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User, Token = httpContext.Request.Headers["Authorization"] });

            services.AddAuthentication("Bearer")
                   .AddIdentityServerAuthentication("Bearer", options =>
                   {
                       options.Authority = _config["IdentityManager:ServerUrl"];
                       options.RequireHttpsMetadata = _config.GetValue<bool>("IdentityManager:RequireHttpsMetadata");
                    });
            
            services
                .AddMemberClient(_config);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseRouting();

            app.UseAuthentication();

            app.UseGraphQL<SportAdminSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
                });
            });
        }
    }
}
