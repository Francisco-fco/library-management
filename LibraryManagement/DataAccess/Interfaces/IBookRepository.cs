using LibraryManagement.Common.Models;

namespace LibraryManagement.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        List<Book> GetAvailableBooks();
        Book? GetBookById(int bookId);
    }
}
