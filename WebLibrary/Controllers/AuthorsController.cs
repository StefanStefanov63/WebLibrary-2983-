using Microsoft.AspNetCore.Mvc;
using WebLibrary.Models.Services.Classes;
using WebLibrary.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebLibrary.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    [Route("Authors")]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthorServices authorService;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
            this.authorService = new AuthorServices(_context);
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("DeleteAuthorByName")]
        public IActionResult DeleteAuthorByName(string aName)
        {
            int output = this.authorService.DeleteAuthorByName(aName);
            return View(output);
        }
        [Route("GetAllAuthors")]
        public IActionResult GetAllAuthors()
        {
            var authors = this.authorService.GetAllAuthors();
            return View(authors);
        }
        [Route("RegisterNewAuthor")]
        public IActionResult RegisterNewAuthor(string aName)
        {
            int output = this.authorService.RegisterNewAuthor(aName);
            return View(output);
        }
        [Route("UpdateAuthorName")]
        public IActionResult UpdateAuthorName(string aOldName, string aNewName)
        {
            int output = this.authorService.UpdateAuthorName(aOldName, aNewName);
            return View(output);
        }
    }
}
