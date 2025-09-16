using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Common.Helpers;
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
                    case "3":
                        ShowMyBorrowedBooks();
                        break;
                    case "4":
                        ReturnBook();
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
            Console.WriteLine("3. Show my borrowed books");
            Console.WriteLine("4. Return book");
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
            if (_currentMember == null) return;

            Console.WriteLine("\n=== Borrow a Book ===");
            
            ShowAvailableBooks();

            List<Book> availableBooks = _libraryManager.GetAvailableBooks();
            if (!availableBooks.Any())
            {
                return;
            }

            Console.Write("\nEnter book ID to borrow: ");
            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                BorrowingResult result = _libraryManager.BorrowBookWithValidation(_currentMember, bookId);

                Console.WriteLine($"\nResult: {result.Message}");
                
                if (result.Success)
                {
                    int newRemainingCapacity = LoanCalculator.GetRemainingBorrowCapacity(result.CurrentBooks.Count, _currentMember.IsPremium);
                    Console.WriteLine($"Remaining borrowing capacity: {newRemainingCapacity}");
                }
            }
            else
            {
                Console.WriteLine("Invalid book ID.");
            }
        }

        private void ShowMyBorrowedBooks()
        {
            if (_currentMember == null) return;

            Console.WriteLine("\n=== My Borrowed Books ===");
            
            List<BorrowedBook> borrowedBooks = _libraryManager.GetMemberBorrowedBooks(_currentMember.MemberId);
            
            if (!borrowedBooks.Any())
            {
                Console.WriteLine("You have no borrowed books.");
                return;
            }

            Console.WriteLine("Book ID | Title                                    | Borrowed Date | Due Date   | Status");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            
            foreach (BorrowedBook borrowedBook in borrowedBooks)
            {
                string status = DateTime.Now > borrowedBook.DueDate ? "OVERDUE" : "On Time";
                int daysOverdue = DateTime.Now > borrowedBook.DueDate ? 
                    (DateTime.Now - borrowedBook.DueDate).Days : 0;
                
                if (daysOverdue > 0)
                {
                    status = $"OVERDUE ({daysOverdue} days)";
                }

                Console.WriteLine($"{borrowedBook.BookId,7} | {borrowedBook.BookTitle,-40} | {borrowedBook.BorrowDate:yyyy-MM-dd}    | {borrowedBook.DueDate:yyyy-MM-dd} | {status}");
            }

            Console.WriteLine($"\nTotal borrowed books: {borrowedBooks.Count}");
            
            int maxBooks = LoanCalculator.GetMaxBooksForMember(_currentMember.IsPremium);
            int remainingCapacity = LoanCalculator.GetRemainingBorrowCapacity(borrowedBooks.Count, _currentMember.IsPremium);
            
            Console.WriteLine($"Borrowing capacity: {borrowedBooks.Count}/{maxBooks} (Remaining: {remainingCapacity})");
            
            List<BorrowedBook> overdueBooks = borrowedBooks.Where(b => DateTime.Now > b.DueDate).ToList();
            if (overdueBooks.Any())
            {
                decimal totalLateFees = 0;
                foreach (BorrowedBook overdueBook in overdueBooks)
                {
                    int daysLate = (DateTime.Now - overdueBook.DueDate).Days;
                    decimal lateFee = daysLate * FeeCalculator.LateFeePerDayPerBook;
                    totalLateFees += lateFee;
                }
                Console.WriteLine($"\nTotal potential late fees: {totalLateFees:C}");
            }
        }

        private void ReturnBook()
        {
            if (_currentMember == null) return;

            Console.WriteLine("\n=== Return a Book ===");
            
            List<BorrowedBook> borrowedBooks = _libraryManager.GetMemberBorrowedBooks(_currentMember.MemberId);
            
            if (!borrowedBooks.Any())
            {
                Console.WriteLine("You have no books to return.");
                return;
            }

            Console.WriteLine("\nYour borrowed books:");
            Console.WriteLine("Book ID | Title                                    | Due Date   | Status");
            Console.WriteLine("-------------------------------------------------------------------------");
            
            foreach (BorrowedBook borrowedBook in borrowedBooks)
            {
                string status = DateTime.Now > borrowedBook.DueDate ? "OVERDUE" : "On Time";
                int daysOverdue = DateTime.Now > borrowedBook.DueDate ? 
                    (DateTime.Now - borrowedBook.DueDate).Days : 0;
                
                if (daysOverdue > 0)
                {
                    status = $"OVERDUE ({daysOverdue} days)";
                }

                Console.WriteLine($"{borrowedBook.BookId,7} | {borrowedBook.BookTitle,-40} | {borrowedBook.DueDate:yyyy-MM-dd} | {status}");
            }

            Console.Write("\nEnter book ID to return: ");
            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                ReturnResult result = _libraryManager.ReturnBookWithValidation(_currentMember, bookId);

                Console.WriteLine($"\nResult: {result.Message}");
                
                if (result.Success && result.LateFee > 0)
                {
                    Console.WriteLine($"Note: A late fee of {result.LateFee:C} has been added to your account.");
                    Console.WriteLine($"Your new outstanding balance will be updated accordingly.");
                }
            }
            else
            {
                Console.WriteLine("Invalid book ID.");
            }
        }
    }
}
