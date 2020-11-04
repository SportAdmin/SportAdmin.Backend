using GraphQLManager.Interface;
using GraphQLManager.Repository;
using GraphQLManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLManager.Extensions
{
    public static class RepositoryGrpcSportAdminExtensions
    {
        public static IServiceCollection AddSportAdminGrpcRepository(this IServiceCollection build)
        {
            return build.AddScoped<IMemberRepository, MemberGrpcRepository>();
        }
    }
}
