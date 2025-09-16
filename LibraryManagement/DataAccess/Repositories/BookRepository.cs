using LibraryManagement.Common.Models;
using LibraryManagement.Common.Helpers;
using LibraryManagement.DataAccess.Interfaces;

namespace LibraryManagement.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books;
        private readonly List<BorrowedBook> _borrowedBooks;
        private readonly Dictionary<int, int> _bookToMemberMapping;

        public BookRepository()
        {
            _books = InitializeBooks();
            _bookToMemberMapping = InitializeBookMemberMapping();
            _borrowedBooks = InitializeBorrowedBooks();
        }

        private List<Book> InitializeBooks()
        {
            return new List<Book>
            {
                new Book { BookId = 1, Title = "Harry Potter and the Philosopher's Stone", IsAvailable = true },
                new Book { BookId = 2, Title = "Lord of the Rings", IsAvailable = false },      
                new Book { BookId = 3, Title = "IT", IsAvailable = true },
                new Book { BookId = 4, Title = "Don Quijote", IsAvailable = false },           
                new Book { BookId = 5, Title = "The Alchemist", IsAvailable = true },
                new Book { BookId = 6, Title = "The Hobbit", IsAvailable = false },            
                new Book { BookId = 7, Title = "The Da Vinci Code", IsAvailable = false },     
                new Book { BookId = 8, Title = "Hippie", IsAvailable = true },
                new Book { BookId = 9, Title = "To kill a Mockingbird", IsAvailable = false }, 
                new Book { BookId = 10, Title = "The Odyssey", IsAvailable = true },
                new Book { BookId = 11, Title = "Hamlet", IsAvailable = false },               
            };
        }

        private Dictionary<int, int> InitializeBookMemberMapping()
        {
            return new Dictionary<int, int>
            {
                { 4, 1 },   
                { 2, 2 },   
                { 6, 2 },   
                { 7, 2 },   
                { 9, 2 },   
                { 11, 2 }   
            };
        }

        private List<BorrowedBook> InitializeBorrowedBooks()
        {
            return new List<BorrowedBook>
            {
                new BorrowedBook
                {
                    BookId = 4,
                    BookTitle = "Don Quijote",
                    BorrowDate = DateTime.Now.AddDays(-16),
                    DueDate = DateTime.Now.AddDays(-2), 
                    IsReturned = false
                },
                
                new BorrowedBook
                {
                    BookId = 2,
                    BookTitle = "Lord of the Rings",
                    BorrowDate = DateTime.Now.AddDays(-22),
                    DueDate = DateTime.Now.AddDays(-8),
                    IsReturned = false
                },
                new BorrowedBook
                {
                    BookId = 6,
                    BookTitle = "The Hobbit",
                    BorrowDate = DateTime.Now.AddDays(-26),
                    DueDate = DateTime.Now.AddDays(-12),
                    IsReturned = false
                },
                new BorrowedBook
                {
                    BookId = 7,
                    BookTitle = "The Da Vinci Code",
                    BorrowDate = DateTime.Now.AddDays(-19),
                    DueDate = DateTime.Now.AddDays(-5),
                    IsReturned = false
                },
                new BorrowedBook
                {
                    BookId = 9,
                    BookTitle = "To kill a Mockingbird",
                    BorrowDate = DateTime.Now.AddDays(-17),
                    DueDate = DateTime.Now.AddDays(-3),
                    IsReturned = false
                },
                new BorrowedBook
                {
                    BookId = 11,
                    BookTitle = "Hamlet",
                    BorrowDate = DateTime.Now.AddDays(-16),
                    DueDate = DateTime.Now.AddDays(-2),
                    IsReturned = false
                }
            };
        }

        public List<Book> GetAllBooks()
        {
            return _books;
        }

        public List<Book> GetAvailableBooks()
        {
            return _books.Where(b => b.IsAvailable).ToList();
        }

        public Book? GetBookById(int bookId)
        {
            return _books.FirstOrDefault(b => b.BookId == bookId);
        }

        public List<BorrowedBook> GetMemberBorrowedBooks(int memberId)
        {
            List<int> memberBookIds = GetMemberBookIds(memberId);

            return _borrowedBooks
                .Where(b => !b.IsReturned && memberBookIds.Contains(b.BookId))
                .ToList();
        }

        public List<BorrowedBook> GetMemberOverdueBooks(int memberId)
        {
            List<int> memberBookIds = GetMemberBookIds(memberId);

            return _borrowedBooks
                .Where(b => !b.IsReturned
                         && memberBookIds.Contains(b.BookId)
                         && DateTime.Now.Date > b.DueDate.Date)
                .ToList();
        }

    

        private List<int> GetMemberBookIds(int memberId)
        {
            return _bookToMemberMapping
                .Where(kvp => kvp.Value == memberId)
                .Select(kvp => kvp.Key)
                .ToList();
        }

     
    }
}
