using WebLibrary.Models.Services.ViewModelsClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLibrary.Models.Services.Interfaces
{
    public interface ILogServices
    {
        int TakeBook(string aBookTitle, string aLibraryCardName);
        int ReturnBook(string aBookTitle, string aLibraryCardName);
        ICollection<LogViewModel> GetAllLogsByUserName(string aName);
        ICollection<LogViewModel> GetAllLogs();
        ICollection<LogViewModel> GetAllLogsByBookTitle(string aTitle);
    }
}
