using LibraryManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Buisness.Interfaces
{
    public interface ILibraryManager
    {
        BorrowingResult? BorrowBook(Member member, Book book, List<BorrowedBook> currentBooks);
        ReturnResult? ReturnBook(BorrowedBook borrowedBook, DateTime returnDate);
        List<Book> GetAllBooks();
        List<Book> GetAvailableBooks();
        Book? GetBookById(int bookId);
        List<Member> GetAllMembers();
        Member? GetMemberById(int memberId);
        List<BorrowedBook> GetMemberBorrowedBooks(int memberId);        
        BorrowingResult BorrowBookWithValidation(Member member, int bookId);
        ReturnResult ReturnBookWithValidation(Member member, int bookId);
    }
}
