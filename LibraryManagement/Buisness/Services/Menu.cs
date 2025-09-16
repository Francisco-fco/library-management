using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Common.Models;

namespace LibraryManagement.Buisness.Services
{
    public class Menu
    {
        private readonly ILibraryManager _libraryManager;
        private Member? _currentMember;

        public Menu(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }
        public void SelectMember()
        {
            Console.WriteLine("\n=== Select Member ===");

            List<Member> members = _libraryManager.GetAllMembers();
            if (members == null || members.Count == 0)
            {
                Console.WriteLine("Inga medlemmar hittades.");
                return;
            }

            foreach (Member member in members)
            {
                string memberType = member.IsPremium ? "Premium" : "Regular";
                string fees = member.OutstandingFees > 0m ? $" - Skuld: {member.OutstandingFees:C}" : string.Empty;
                Console.WriteLine($"{member.MemberId}. {member.Name} ({memberType}){fees}");
            }

            Console.Write("Enter member ID: ");
            if (int.TryParse(Console.ReadLine(), out int memberId))
            {
                _currentMember = _libraryManager.GetMemberById(memberId);
                if (_currentMember != null)
                {
                    Console.WriteLine($"Welcome, {_currentMember.Name}!");
                }
                else
                {
                    Console.WriteLine("Member not found. Using default member.");
                    _currentMember = _libraryManager.GetAllMembers().First();
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Using default member.");
                _currentMember = _libraryManager.GetAllMembers().First();
            }
        }
    }
}
