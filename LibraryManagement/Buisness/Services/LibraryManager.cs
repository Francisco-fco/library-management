using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Common.Models;

namespace LibraryManagement.Buisness.Services
{
    public class LibraryManager : ILibraryManager
    {
        public BorrowingResult? BorrowBook(Member member, Book book, List<BorrowedBook> currentBooks)
        {
            Console.WriteLine("BorrowBook method called");
            return null;
        }

        public ReturnResult? ReturnBook(BorrowedBook borrowedBook, DateTime returnDate)
        {
            Console.WriteLine("ReturnBook method called");
            return null;
        }
    }
}