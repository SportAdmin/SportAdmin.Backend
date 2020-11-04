using GraphQLManager.GraphQLOperation.Type.Member;
using System.Threading.Tasks;

namespace GraphQLManager.Interface
{
    public interface IMemberRepository
    {
        Task<MemberItem> GetMemberAsync(string id);
    }
}