using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace MemberManager
{
    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme)]
    public class GreeterService : Members.MembersBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<MemberReply> getMember(MemberRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MemberReply() { FirstName = "Fredrik", LastName = "Larsson" });
        }
    }
}
