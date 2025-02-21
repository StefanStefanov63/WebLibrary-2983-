using WebLibrary.Data;
using WebLibrary.Models.Services.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApplication.Controllers
{
    [Authorize]
    [Route("Logs")]
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LogServices logServices;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
            this.logServices = new LogServices(_context);
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("GetMyLogs")]
        public IActionResult GetMyLogs()
        {
            var logs = logServices.GetAllLogsByUserName(User.Identity.Name);
            return View(logs);
        }
        [Route("GetAllLogs")]
        [Authorize(Roles = "Librarian,Admin")]
        public IActionResult GetAllLogs()
        {
            var logs = logServices.GetAllLogs();
            return View(logs);
        }
        [Authorize(Roles = "Librarian,Admin")]
        [Route("GetAllLogsByUserName")]
        public IActionResult GetAllLogsByUserName(string anUserName)
        {
            var logs = logServices.GetAllLogsByUserName(anUserName);
            return View(logs);
        }
        [Authorize(Roles = "Librarian,Admin")]
        [Route("GetAllLogsByBookTitle")]
        public IActionResult GetAllLogsByBookTitle(string aTitle)
        {
            var logs = logServices.GetAllLogsByBookTitle(aTitle);
            return View(logs);
        }
        [Authorize(Roles = "Librarian,Admin")]
        [Route("TakeBook")]
        public IActionResult TakeBook(string aBookTitle, string aUserName)
        {
            int output = this.logServices.TakeBook(aBookTitle, aUserName);
            return View(output);
        }
        [Authorize(Roles = "Librarian,Admin")]
        [Route("ReturnBook")]
        public IActionResult ReturnBook(string aBookTitle, string aUserName)
        {
            int output = this.logServices.ReturnBook(aBookTitle, aUserName);
            return View(output);
        }
    }
}
