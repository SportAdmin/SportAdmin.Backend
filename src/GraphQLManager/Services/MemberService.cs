using GraphQL;
using GraphQLManager.GraphQLOperation;
using GraphQLManager.GraphQLOperation.Type.Member;
using GraphQLManager.Interface;
using MemberManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLManager.Services
{
    public class MemberService : IMemberService
    {        
        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public IMemberRepository _repository { get; }

        public async Task<MemberItem> GetMemberAsync(string id)
        {
            return await _repository.GetMemberAsync(id);
        }
    }
}
