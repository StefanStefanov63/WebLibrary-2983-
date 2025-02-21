using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLibrary.Data;
using WebLibrary.Models.Services.Classes;

namespace WebLibrary.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Users")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserServices userService;
        private readonly IServiceScopeFactory _scopeFactory;

        public UsersController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
        {

            _context = context;
            this.userService = new UserServices(_context);
            this._scopeFactory = scopeFactory;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("ChangeUserRole")]
        public IActionResult ChangeUserRole(string aUserName, string anOldRole, string aNewRole)
        {
            int output = this.userService.ChangeUserRole(aUserName,anOldRole,aNewRole,this._scopeFactory).Result;
            return View(output);
        }
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = userService.GetAllUsers();
            return View(users);
        }
        [Route("GetAllUsersByRole")]
        public IActionResult GetAllUsersByRole(string aRole)
        {
            var users = userService.GetAllUsersByRole(aRole);
            return View(users);
        }
    }
}
