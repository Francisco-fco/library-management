using LibraryManagement.Common.Models;

namespace LibraryManagement.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        List<Book> GetAvailableBooks();
        Book? GetBookById(int bookId);
        List<BorrowedBook> GetMemberBorrowedBooks(int memberId);
        List<BorrowedBook> GetMemberOverdueBooks(int memberId);
    }
}
