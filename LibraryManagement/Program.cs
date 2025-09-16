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
        IMemberRepository memberRepository = new MemberRepository();
        
        ILibraryManager libraryManager = new LibraryManager(bookRepository, memberRepository);
        
        Menu menu = new Menu(libraryManager);
        
        menu.RunApplication();
    }
}