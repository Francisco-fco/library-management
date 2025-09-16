using LibraryManagement.Common.Models;

namespace LibraryManagement.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        Book? GetBookById(int bookId);
    }
}
