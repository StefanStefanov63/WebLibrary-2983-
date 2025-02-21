using WebLibrary.Models.Services.ViewModelsClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLibrary.Models.Services.Interfaces
{
    public interface IAuthorServices
    {
        int RegisterNewAuthor(string aName);
        int DeleteAuthorByName(string aName);
        int UpdateAuthorName(string aOldName, string aNewName);
        ICollection<AuthorViewModel> GetAllAuthors();
    }
}
