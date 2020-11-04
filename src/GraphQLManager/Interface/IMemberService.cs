using GraphQLManager.GraphQLOperation.Type.Member;
using System.Threading.Tasks;

namespace GraphQLManager.Interface
{
    public interface IMemberService
    {
        Task<MemberItem> GetMemberAsync(string id);
    }
}