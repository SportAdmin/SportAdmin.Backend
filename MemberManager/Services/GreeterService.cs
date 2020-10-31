using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MemberManager
{
    public class GreeterService : Members.MembersBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<MemberReply> getMember(MemberRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MemberReply() { Firstname = "Fredrik", Lastname = "Larsson" });
        }
    }
}
