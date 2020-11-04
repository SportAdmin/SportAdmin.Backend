using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MemberManager.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddSportAdminAuthentication(this AuthenticationBuilder build, IConfiguration config)
        {
            return build.AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = config["IdentityManager:ServerUrl"];
                    options.RequireHttpsMetadata = config.GetValue<bool>("IdentityManager:RequireHttpsMetadata");
                })
                .AddCertificate(opt =>
                {
                    opt.AllowedCertificateTypes = CertificateTypes.SelfSigned;
                    opt.RevocationMode = X509RevocationMode.NoCheck; // Self-Signed Certs (Development)
                    opt.Events = new CertificateAuthenticationEvents()
                    {
                        OnCertificateValidated = ctx =>
                        {
                            // Write additional Validation  
                            ctx.Success();
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
