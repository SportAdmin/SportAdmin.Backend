using GraphQLManager.Interface;
using GraphQLManager.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLManager.Extensions
{
    public static class ServiceSportAdminExtensions
    {
        public static IServiceCollection AddSportAdminService(this IServiceCollection build)
        {
            return build.AddScoped<IMemberService, MemberService>();
        }
    }
}
