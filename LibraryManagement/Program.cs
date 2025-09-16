using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Buisness.Services;
using LibraryManagement.DataAccess.Interfaces;
using LibraryManagement.DataAccess.Repositories;
using LibraryManagement.Common.Models;

class Program
{
    static void Main()
    {
        IBookRepository bookRepository = new BookRepository();

        ILibraryManager libraryManager = new LibraryManager(bookRepository);

        List<Book> allBooks = libraryManager.GetAllBooks();

        Console.WriteLine("Alla böcker i biblioteket:");
        Console.WriteLine("=" + new string('=', 40));

        foreach (Book book in allBooks)
        {
            string status = book.IsAvailable ? "Tillgänglig" : "Utlånad";
            Console.WriteLine($"ID: {book.BookId,-3} | {book.Title,-40} | Status: {status}");
        }

        Console.WriteLine($"\nTotalt antal böcker: {allBooks.Count}");
        Console.WriteLine($"Tillgängliga böcker: {allBooks.Count(b => b.IsAvailable)}");
        Console.WriteLine($"Utlånade böcker: {allBooks.Count(b => !b.IsAvailable)}");
        
        List<Book> availableBooks = libraryManager.GetAvailableBooks();

        foreach (Book book in availableBooks)
        {
            string status = book.IsAvailable ? "Tillgänglig" : "Utlånad";
            Console.WriteLine($"ID: {book.BookId,-3} | {book.Title,-40} | Status: {status}");
        }

        Console.WriteLine($"\nEndast tillgängliga böcker: {availableBooks}");
    }
}