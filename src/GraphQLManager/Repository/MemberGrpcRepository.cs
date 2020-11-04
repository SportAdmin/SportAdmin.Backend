using GraphQLManager.GraphQLOperation.Type.Member;
using GraphQLManager.Interface;
using MemberManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLManager.Repository
{
    public class MemberGrpcRepository : IMemberRepository
    {
        private Members.MembersClient _client { get; }

        public MemberGrpcRepository(Members.MembersClient client)
        {
            _client = client;
        }

        public async Task<MemberItem> GetMemberAsync(string id)
        {
            var reply = await _client.getMemberAsync(new MemberRequest { Id = id });

            MemberItem item = new MemberItem()
            {
                Id = reply.Id,
                FirstName = reply.FirstName,
                LastName = reply.LastName,
                Born = reply.Born?.ToDateTime() ?? new DateTime(),
                Email = reply.Email,
                Street = reply.Street,
                ZIP = reply.ZIP,
                City = reply.City
            };

            return item;
        }
    }
}
