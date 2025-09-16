using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Common.Helpers;
using LibraryManagement.Common.Models;

namespace LibraryManagement.Buisness.Services
{
    public class Menu : IMenu
    {
        private readonly ILibraryManager _libraryManager;
        private Member? _currentMember;

        public Menu(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }

        public void RunApplication()
        {
            Console.WriteLine("== Library Management ==");

            SelectMember();
            bool running = true;

            while (running)
            {
                ShowMainMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAvailableBooks();
                        break;
                    case "2":
                        BorrowBook();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Thank you for using Library Management System!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.Clear();

            if (_currentMember == null)
            {
                Console.WriteLine("== Library Management System ==");
                Console.WriteLine("No member selected. Please select a member first.");
                return;
            }

            string memberType = _currentMember.IsPremium ? "Premium" : "Regular";

            Console.WriteLine($"== Library Management System - {_currentMember.Name} ({memberType}) ==");
            Console.WriteLine("1. Show available books");
            Console.WriteLine("2. Borrow book");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
        }
        
        private void SelectMember()
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
        
        private void ShowAvailableBooks()
        {
            if (_currentMember == null) return;

            Console.WriteLine("\n=== Available Books ===");
            List<Book> availableBooks = _libraryManager.GetAvailableBooks();

            if (!availableBooks.Any())
            {
                Console.WriteLine("No books are currently available.");
                return;
            }

            Console.WriteLine("ID | Title");
            Console.WriteLine("---|---------------------------------------");
            foreach (Book book in availableBooks)
            {
                Console.WriteLine($"{book.BookId,2} | {book.Title}");
            }
        }
        
        private void BorrowBook() 
        {
            Console.WriteLine("\nBorrow Book ");
        }
    }
}
