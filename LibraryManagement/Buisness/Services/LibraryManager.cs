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
        private readonly Dictionary<int, List<BorrowedBook>> _memberBorrowedBooks;

        public LibraryManager(IBookRepository bookRepository, IMemberRepository memberRepository)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _memberBorrowedBooks = new Dictionary<int, List<BorrowedBook>>();
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
            if (!_memberBorrowedBooks.ContainsKey(memberId))
            {
                return new List<BorrowedBook>();
            }
            
            return _memberBorrowedBooks[memberId].Where(b => !b.IsReturned).ToList();
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

            if (!book.IsAvailable)
            {
                return new BorrowingResult
                {
                    Success = false,
                    Message = "Book is not available for borrowing.",
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

            // Add to member's borrowed books
            if (!_memberBorrowedBooks.ContainsKey(member.MemberId))
            {
                _memberBorrowedBooks[member.MemberId] = new List<BorrowedBook>();
            }
            _memberBorrowedBooks[member.MemberId].Add(borrowedBook);
            currentBooks.Add(borrowedBook);

            book.IsAvailable = false;

            return new BorrowingResult
            {
                Success = true,
                Message = $"Successfully borrowed '{book.Title}'. Due date: {borrowedBook.DueDate:yyyy-MM-dd}",
                CurrentBooks = currentBooks
            };
        }

        public ReturnResult ReturnBookWithValidation(Member member, int bookId)
        {
            if (!_memberBorrowedBooks.ContainsKey(member.MemberId))
            {
                return new ReturnResult
                {
                    Success = false,
                    Message = "No active loans found for this member.",
                    LateFee = 0,
                    DaysLate = 0
                };
            }

            BorrowedBook? borrowedBook = _memberBorrowedBooks[member.MemberId]
                .FirstOrDefault(b => b.BookId == bookId && !b.IsReturned);

            if (borrowedBook == null)
            {
                return new ReturnResult
                {
                    Success = false,
                    Message = "No active loan found for this book.",
                    LateFee = 0,
                    DaysLate = 0
                };
            }

            DateTime returnDate = DateTime.Now;
            int daysLate = 0;
            decimal lateFee = 0;

            if (returnDate > borrowedBook.DueDate)
            {
                daysLate = (returnDate - borrowedBook.DueDate).Days;
                lateFee = daysLate * FeeCalculator.LateFeePerDayPerBook;
            }

            borrowedBook.IsReturned = true;

            Book? book = GetBookById(bookId);
            if (book != null)
            {
                book.IsAvailable = true;
            }

            string message = $"Successfully returned '{borrowedBook.BookTitle}'.";
            if (daysLate > 0)
            {
                message += $" Book was {daysLate} day(s) late. Late fee: {lateFee:C}";
            }

            return new ReturnResult
            {
                Success = true,
                Message = message,
                LateFee = lateFee,
                DaysLate = daysLate
            };
        }
    }
}