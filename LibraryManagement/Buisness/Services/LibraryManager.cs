using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Common.Models;
using LibraryManagement.DataAccess.Interfaces;

namespace LibraryManagement.Buisness.Services
{
    public class LibraryManager : ILibraryManager
    {
        private readonly IBookRepository _bookRepository;

        public LibraryManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

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

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public List<Book> GetAvailableBooks()
        {
            return _bookRepository.GetAvailableBooks();
        }
    }
}