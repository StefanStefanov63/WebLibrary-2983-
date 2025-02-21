using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary.Models.Services.Interfaces;
using WebLibrary.Models.Services.ViewModelsClasses;
using WebLibrary.Data;
using WebLibrary.Models.Entities;

namespace WebLibrary.Models.Services.Classes
{
    public class LogServices : ILogServices
    {
        private readonly ApplicationDbContext db;

        public LogServices(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ICollection<LogViewModel> GetAllLogs()
        {
            var aLogs = db.Logs.ToList();
            var allLogs = new List<LogViewModel>();
            foreach (var aLog in aLogs)
            {
                LogViewModel aLogViewModel = new LogViewModel();
                aLogViewModel.Id = aLog.Id;
                aLogViewModel.IsReturned = aLog.IsReturned;
                aLogViewModel.BookTitle = db.Books.FirstOrDefault(x => x.Id == aLog.BookId).Title;
                aLogViewModel.UserName = db.Users.FirstOrDefault(x => x.Id == aLog.UserId).UserName;
                allLogs.Add(aLogViewModel);
            }
            return allLogs;
        }

        public ICollection<LogViewModel> GetAllLogsByBookTitle(string aTitle)
        {
            var aLogs = db.Logs.ToList();
            var allLogs = new List<LogViewModel>();
            foreach (var aLog in aLogs)
            {
                string aLogBookTitle = db.Books.FirstOrDefault(x => x.Id == aLog.BookId).Title;
                if (aLogBookTitle == aTitle)
                {
                    LogViewModel aLogViewModel = new LogViewModel();
                    aLogViewModel.Id = aLog.Id;
                    aLogViewModel.IsReturned = aLog.IsReturned;
                    aLogViewModel.BookTitle = aLogBookTitle;
                    aLogViewModel.UserName = db.Users.FirstOrDefault(x => x.Id == aLog.UserId).UserName;
                    allLogs.Add(aLogViewModel);
                }

            }
            return allLogs;
        }

        public ICollection<LogViewModel> GetAllLogsByUserName(string aUserName)
        {
            var aLogs = GetAllLogs();
            var allLogs = new List<LogViewModel>();
            foreach (var aLog in aLogs)
            {
                string userName = aLog.UserName;
                if (userName == aUserName)
                {
                    LogViewModel aLogViewModel = new LogViewModel();
                    aLogViewModel.Id = aLog.Id;
                    aLogViewModel.IsReturned = aLog.IsReturned;
                    aLogViewModel.BookTitle = aLog.BookTitle;
                    aLogViewModel.UserName = userName;
                    allLogs.Add(aLogViewModel);
                }

            }
            return allLogs;
        }
        public int ReturnBook(string aBookTitle, string aUserName)
        {
			var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aBookTitle.Trim());
			var aUser = db.Users.FirstOrDefault(x => x.UserName == aUserName);
			if (aBook is null && aUser is null)
			{
				return 1;
			}
			else if (aBook is null)
			{
				return 2;
			}
			else if (aUser is null)
			{
				return 3;
			}
			else
			{
				var aLog = db.Logs.FirstOrDefault(x => x.BookId == aBook.Id && x.UserId == aUser.Id && x.IsReturned == false);
				if (aLog is null)
				{
					return 4;
				}
				else
				{

					aLog.IsReturned = true;
					aBook.Quantity++;
					db.SaveChanges();
					return 5;
				}
			}
		}

        public int TakeBook(string aBookTitle, string aUserName)
        {
            var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aBookTitle.Trim());
            var aUser = db.Users.FirstOrDefault(x => x.UserName == aUserName);
            if (aBook == null && aUser == null)
            {
				return 1;
			}
            else if (aBook == null)
            {
				return 2;
			}
            else if (aUser == null)
            {
				return 3;
			}
            else
            {
                if (aBook.Quantity > 0)
                {
                    aBook.Quantity--;
                    Log log = new Log();
                    log.IsReturned = false;
                    log.BookId = aBook.Id;
                    log.UserId = aUser.Id;
                    db.Logs.Add(log);
                    db.SaveChanges();
					return 4;
				}
                else
                {
					return 5;
				}
            }
            
        }
    }
}
