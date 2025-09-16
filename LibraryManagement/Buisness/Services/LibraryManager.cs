using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Common.Models;
using LibraryManagement.DataAccess.Interfaces;

namespace LibraryManagement.Buisness.Services
{
    public class LibraryManager : ILibraryManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;

        public LibraryManager(IBookRepository bookRepository, IMemberRepository memberRepository)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
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

        public Book? GetBookById(int bookId)
        {
            return _bookRepository.GetBookById(bookId);
        }

        public List<Member> GetAllMembers()
        {
            return _memberRepository.GetAllMembers();
        }

        public Member? GetMemberById(int memberId)
        {
            return _memberRepository.GetMemberById(memberId);
        }
    }
}