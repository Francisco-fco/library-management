using LibraryManagement.Buisness.Interfaces;
using LibraryManagement.Buisness.Services;
using LibraryManagement.DataAccess.Interfaces;
using LibraryManagement.DataAccess.Repositories;
using LibraryManagement.Common.Models;

class Program
{
    static void Main()
    {
        // Skapa repository instanser
        IBookRepository bookRepository = new BookRepository();
        IMemberRepository memberRepository = new MemberRepository();
        
        // Skapa LibraryManager instans med dependency injection
        ILibraryManager libraryManager = new LibraryManager(bookRepository, memberRepository);
        
        // Visa alla böcker
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
        
        // Visa alla medlemmar
        List<Member> allMembers = libraryManager.GetAllMembers();
        
        Console.WriteLine("\n\nAlla medlemmar i biblioteket:");
        Console.WriteLine("=" + new string('=', 50));
        
        foreach (Member member in allMembers)
        {
            string memberType = member.IsPremium ? "Premium" : "Standard";
            Console.WriteLine($"ID: {member.MemberId,-3} | {member.Name,-15} | Typ: {memberType,-8} | Skuld: {member.OutstandingFees:C}");
        }
        
        Console.WriteLine($"\nTotalt antal medlemmar: {allMembers.Count}");
        Console.WriteLine($"Premium medlemmar: {allMembers.Count(m => m.IsPremium)}");
        Console.WriteLine($"Standard medlemmar: {allMembers.Count(m => !m.IsPremium)}");
        Console.WriteLine($"Total utestående skuld: {allMembers.Sum(m => m.OutstandingFees):C}");
    }
}