using WebLibrary.Models.Services.ViewModelsClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLibrary.Models.Services.Interfaces
{
    public interface IBookServices
    {
        int[] RegisterNewBook(string aTitle, string anAuthor, string aDescription, int aQuantity);
        int DeleteBookByTitle(string aTitle);
        int UpdateBookTitle(string anOldTitle, string aNewTitle);
        int UpdateBookAuthor(string aTitle, string aNewAuthor);
        int UpdateBookDescription(string aTitle, string aNewDescription);
        int UpdateBookQuantity(string aTitle, int aNewQuantity);
        int[] UpdateBook(string anOldTitle, string aNewTitle, string aNewAuthor, string aNewDescription, int aNewQuantity);
        ICollection<BookViewModel> GetAllBooks();
        BookViewModel GetBookByTitle(string aTitle);
        ICollection<BookViewModel> GetAllBookFromAuthor(string amAuthor);
    }
}
