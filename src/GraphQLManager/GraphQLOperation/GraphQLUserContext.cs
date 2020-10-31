using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQLManager.GraphQLOperation
{
    public class GraphQLUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get;  set; }
    }

    public interface IProvideClaimsPrincipal
    {
        ClaimsPrincipal User { get; }
    }
}
