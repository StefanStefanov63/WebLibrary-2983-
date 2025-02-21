using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebLibrary.Data;
using WebLibrary.Models.Services.Interfaces;
using WebLibrary.Models.Services.ViewModelsClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebLibrary.Models.Services.Classes
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext db;

        public UserServices(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<int> ChangeUserRole(string aUserName, string anOldRole,string aNewRole, IServiceScopeFactory aScopeFactory)
        {
            using (var scope = aScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider;
                var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = service.GetRequiredService<UserManager<IdentityUser>>();

                var user = await userManager.FindByNameAsync(aUserName);
                if (user != null)
                {
                    if (anOldRole != aNewRole)
                    {
                        if (await userManager.IsInRoleAsync(user, anOldRole))
                        {
                            await userManager.RemoveFromRoleAsync(user, anOldRole);
                        }

                        if (!await userManager.IsInRoleAsync(user, aNewRole))
                        {
                            await userManager.AddToRoleAsync(user, aNewRole);
                        }
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
                
            }
        }

        public ICollection<UserViewModel> GetAllUsers()
        {
            var aUsers = db.Users.ToList();
            var allUsers = new List<UserViewModel>();
            foreach (var aUser in aUsers)
            {
                UserViewModel aUserViewModel = new UserViewModel();
                aUserViewModel.UserName = aUser.UserName;
                aUserViewModel.Role = db.Roles.FirstOrDefault(r => r.Id == (db.UserRoles.FirstOrDefault(u => u.UserId == aUser.Id).RoleId)).Name;
                allUsers.Add(aUserViewModel);
            }
            return allUsers;
        }

        public ICollection<UserViewModel> GetAllUsersByRole(string aRole)
        {
            var aUsers = db.Users.ToList();
            var allUsers = new List<UserViewModel>();
            foreach (var aUser in aUsers)
            {
                if (db.Roles.FirstOrDefault(r => r.Id == (db.UserRoles.FirstOrDefault(u => u.UserId == aUser.Id).RoleId)).Name == aRole)
                {
                    UserViewModel aUserViewModel = new UserViewModel();
                    aUserViewModel.UserName = aUser.UserName;
                    aUserViewModel.Role = aRole;
                    allUsers.Add(aUserViewModel);
                }
            }
            return allUsers;
        }
    }
}
