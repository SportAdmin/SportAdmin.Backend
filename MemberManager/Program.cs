using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MemberManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel((hostingContext, opt) =>
                    {
                        bool checkCertificateRevocation = hostingContext.Configuration.GetValue<bool>("Certificate:CheckCertificateRevocation");
                        string certFile = hostingContext.Configuration.GetValue<string>("Certificate:File");
                        string pasword = hostingContext.Configuration.GetValue<string>("Certificate:Password");

                        var cert = new X509Certificate2(certFile, pasword);

                        opt.ConfigureHttpsDefaults(h =>
                        {
                            h.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                            h.CheckCertificateRevocation = checkCertificateRevocation;
                            h.ServerCertificate = cert;
                        });
                    });
                });
    }
}
