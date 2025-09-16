using LibraryManagement.Common.Models;
using LibraryManagement.DataAccess.Interfaces;

namespace LibraryManagement.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books;

        public BookRepository()
        {
            _books = InitializeBooks();
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

        public List<Book> GetAllBooks()
        {
            return _books;
        }

        public Book? GetBookById(int bookId)
        {
            return _books.FirstOrDefault(b => b.BookId == bookId);
        }
    }
}
