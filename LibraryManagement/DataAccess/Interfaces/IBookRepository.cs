using LibraryManagement.Common.Models;

namespace LibraryManagement.DataAccess.Interfaces
{
    internal interface IBookRepository
    {
        List<Book> GetAllBooks();
        Book? GetBookById(int bookId);
    }
}
