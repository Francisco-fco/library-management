using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Common.Models;
using LibraryManagement.Common.Helpers;
using LibraryManagement.DataAccess.Interfaces;

namespace LibraryManagement.Buisness.Services
{
    public class LibraryManager : ILibraryManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly List<BorrowedBook> _borrowedBooks;

        public LibraryManager(IBookRepository bookRepository, IMemberRepository memberRepository)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _borrowedBooks = new List<BorrowedBook>();
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

        public List<BorrowedBook> GetMemberBorrowedBooks(int memberId)
        {
            return _borrowedBooks.Where(b => !b.IsReturned).ToList();
        }

        public BorrowingResult BorrowBookWithValidation(Member member, int bookId)
        {
            List<BorrowedBook> currentBooks = GetMemberBorrowedBooks(member.MemberId);
            
            if (!LoanCalculator.CanMemberBorrowMoreBooks(currentBooks.Count, member.IsPremium))
            {
                int memberMaxBooks = LoanCalculator.GetMaxBooksForMember(member.IsPremium);
                return new BorrowingResult
                {
                    Success = false,
                    Message = $"You have reached your borrowing limit of {memberMaxBooks} books. Please return some books before borrowing new ones.",
                    CurrentBooks = currentBooks
                };
            }

            if (!FeeCalculator.CanMemberBorrow(member.OutstandingFees))
            {
                return new BorrowingResult
                {
                    Success = false,
                    Message = $"You have outstanding fees of {member.OutstandingFees:C}. Please pay down to {FeeCalculator.MaxOutstandingFeesBeforeBorrowBlock:C} or less before borrowing.",
                    CurrentBooks = currentBooks
                };
            }

            Book? book = GetBookById(bookId);
            if (book == null)
            {
                return new BorrowingResult
                {
                    Success = false,
                    Message = "Book not found.",
                    CurrentBooks = currentBooks
                };
            }


            BorrowedBook borrowedBook = new BorrowedBook
            {
                BookId = book.BookId,
                BookTitle = book.Title,
                BorrowDate = DateTime.Now,
                DueDate = FeeCalculator.CalculateDueDate(),
                IsReturned = false
            };

            _borrowedBooks.Add(borrowedBook);
            currentBooks.Add(borrowedBook);

            book.IsAvailable = false;

            return new BorrowingResult
            {
                Success = true,
                Message = $"Successfully borrowed '{book.Title}'. Due date: {borrowedBook.DueDate:yyyy-MM-dd}",
                CurrentBooks = currentBooks
            };
        }
    }
}