using LibraryManagement.Common.Models;
using LibraryManagement.DataAccess.Interfaces;

namespace LibraryManagement.DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly List<Member> _members;

        public MemberRepository()
        {
            _members = InitializeMembers();
        }

        private List<Member> InitializeMembers()
        {
            return new List<Member>
            {
                new Member { MemberId = 1, Name = "Ali", IsPremium = false, OutstandingFees = 10.00m },    
                new Member { MemberId = 2, Name = "Elin", IsPremium = true, OutstandingFees = 150.00m },    
                new Member { MemberId = 3, Name = "Henrik", IsPremium = false, OutstandingFees = 0.00m }    
            };
        }

        public List<Member> GetAllMembers()
        {
            return _members;
        }

        public Member? GetMemberById(int memberId)
        {
            return _members.FirstOrDefault(m => m.MemberId == memberId);
        }

    }
}
