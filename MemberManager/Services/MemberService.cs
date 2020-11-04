using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MemberManager
{
    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme, Policy = "Bearer")]
    public class GreeterService : Members.MembersBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<MemberReply> getMember(MemberRequest request, ServerCallContext context)
        {
            var id = context.GetHttpContext().User.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value;

            return Task.FromResult(new MemberReply() { Id = id, FirstName = "Fredrik", LastName = "Larsson" });
        }
    }
}
