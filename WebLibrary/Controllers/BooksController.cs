using Microsoft.AspNetCore.Mvc;
using WebLibrary.Models.Services.Classes;
using WebLibrary.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebLibrary.Controllers
{
    [Route("Books")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BookServices bookService;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
            this.bookService = new BookServices(_context);
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            var books = bookService.GetAllBooks();
            return View(books);
        }
        [Route("GetAllBookFromAuthor")]
        public IActionResult GetAllBooksFromAuthor(string anAuthor)
        {
            var books = bookService.GetAllBookFromAuthor(anAuthor);
            return View(books);
        }
        [Route("GetBookByTitle")]
        public IActionResult GetBookByTitle(string aTitle)
        {
            var books = bookService.GetBookByTitle(aTitle);
            return View(books);
        }
        [Authorize(Roles = "Librarian,Admin")]
        [Route("RegisterNewBook")]
		public IActionResult RegisterNewBook(string aTitle, string anAuthor, string aDescription, int aQuantity)
		{
			int[] output = this.bookService.RegisterNewBook(aTitle, anAuthor, aDescription, aQuantity);
			return View(output);
		}
        [Authorize(Roles = "Librarian,Admin")]
        [Route("DeleteBookByTitle")]
		public IActionResult DeleteBookByTitle(string aTitle)
		{
			int output = this.bookService.DeleteBookByTitle(aTitle);
			return View(output);
		}
        [Authorize(Roles = "Librarian,Admin")]
        [Route("UpdateBook")]
		public IActionResult UpdateBook(string aCurrentTitle, string aNewTitle, string anAuthor, string aDescription, int aQuantity)
		{
			int[] output = this.bookService.UpdateBook(aCurrentTitle, aNewTitle, anAuthor, aDescription, aQuantity);
			return View(output);
		}
	}
}
