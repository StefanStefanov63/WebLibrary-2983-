using WebLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebLibrary.Data
{
    public static class SeedData
    {
        public static async Task SeedingDbUsersAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                var roles = new string[] { "Admin", "Librarian", "User" };
                foreach (var role in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var adminUser = await userManager.FindByEmailAsync("admin@library.com");
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = "admin@library.com",
                        Email = "admin@library.com"
                    };
                    var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                var librarianUser = await userManager.FindByEmailAsync("librarian@library.com");
                if (librarianUser == null)
                {
                    librarianUser = new IdentityUser
                    {
                        UserName = "librarian@library.com",
                        Email = "librarian@library.com"
                    };
                    var result = await userManager.CreateAsync(librarianUser, "LibrarianPassword123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(librarianUser, "Librarian");
                    }
                }

                var regularUser = await userManager.FindByEmailAsync("user@library.com");
                if (regularUser == null)
                {
                    regularUser = new IdentityUser
                    {
                        UserName = "user@library.com",
                        Email = "user@library.com"
                    };
                    var result = await userManager.CreateAsync(regularUser, "UserPassword123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(regularUser, "User");
                    }
                }

                var seedContext = services.GetRequiredService<ApplicationDbContext>();
                SeedData.SeedingDbBooksLogsAuthors(seedContext);
            }
        }
        public static void SeedingDbBooksLogsAuthors(ApplicationDbContext context)
        {
            
            context.Database.EnsureCreated();
            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                    new Author { Name = "Fizzicks" },
                    new Author { Name = "Nemorosus" },
                    new Author { Name = "Virlyce" },
                    new Author { Name = "nobody103" },
                    new Author { Name = "InadvisablyCompelled" },
                    new Author { Name = "cathfach" },
                    new Author { Name = "Gogglesbear" },
                    new Author { Name = "Yousureimnotarobot" },
                    new Author { Name = "Qi Peijia" },
                    new Author { Name = "TK523" }
                );
                context.SaveChanges();
            }

            // Seed Books and Logs (same as before without IDs for Book and Log)
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book { Title = "Cultist of Cerebon", AuthorId = 1, Quantity = 1 },
                    new Book { Title = "A Nerubian's Journey", AuthorId = 1, Quantity = 3 },
                    new Book { Title = "Jackal Among Snakes", AuthorId = 2, Quantity = 6 },
                    new Book { Title = "A Rational Zombie", AuthorId = 3, Description = "An existentialist zombie seeks the meaning of life one mouthful at a time.", Quantity = 1 },
                    new Book { Title = "Master of the System", AuthorId = 3, Quantity = 1 },
                    new Book { Title = "Zenith of Sorcery", AuthorId = 4, Description = "A new fantasy story from the author of Mother of Learning...", Quantity = 10 },
                    new Book { Title = "Mother of Learning", AuthorId = 4, Quantity = 2 },
                    new Book { Title = "Paranoid Mage", AuthorId = 5, Description = "Callum had seen things all his life. There are monsters and beasts...", Quantity = 4 },
                    new Book { Title = "Chasing Sunlight", AuthorId = 5, Description = "In a world of lightless skies and endless secrets...", Quantity = 8 },
                    new Book { Title = "An Unborn Hero", AuthorId = 6, Quantity = 3 },
                    new Book { Title = "Super Minion", AuthorId = 7, Description = "Fortress City has Super Villains...", Quantity = 7 },
                    new Book { Title = "Human Altered", AuthorId = 8, Quantity = 6 },
                    new Book { Title = "The Legendary Mechanic", AuthorId = 9, Quantity = 11 },
                    new Book { Title = "The Godking's Legacy", AuthorId = 3, Quantity = 5 },
                    new Book { Title = "A Lonely Dungeon", AuthorId = 6, Description = "When a new dungeon is born...", Quantity = 9 },
                    new Book { Title = "Dear Spellbook", AuthorId = 10, Description = "Hello stranger, my name is Tal...", Quantity = 8 },
                    new Book { Title = "An Unwilling Monster", AuthorId = 1, Quantity = 1 },
                    new Book { Title = "The Gemstone Prince", AuthorId = 3, Quantity = 2 },
                    new Book { Title = "A Sect Elder's Journey", AuthorId = 1, Quantity = 3 },
                    new Book { Title = "Primal Wizardry", AuthorId = 10, Quantity = 10 }
                );
                context.SaveChanges();
            }

            // Seed Logs if they don't exist
            string anAdminUserId = context.Users.FirstOrDefault(x => x.UserName == "admin@library.com").Id;
            string anUserUserId = context.Users.FirstOrDefault(x => x.UserName == "user@library.com").Id;
            string aLibrarianUserId = context.Users.FirstOrDefault(x => x.UserName == "librarian@library.com").Id;
            if (!context.Logs.Any())
            {
                context.Logs.AddRange(
                    new Log { IsReturned = false, BookId = 3, UserId = aLibrarianUserId },
                    new Log { IsReturned = false, BookId = 7, UserId = anAdminUserId },
                    new Log { IsReturned = true, BookId = 2, UserId = anUserUserId },
                    new Log { IsReturned = false, BookId = 2, UserId = aLibrarianUserId },
                    new Log { IsReturned = false, BookId = 3, UserId = anUserUserId },
                    new Log { IsReturned = true, BookId = 6, UserId = anAdminUserId },
                    new Log { IsReturned = false, BookId = 8, UserId = anAdminUserId },
                    new Log { IsReturned = true, BookId = 9, UserId = aLibrarianUserId },
                    new Log { IsReturned = true, BookId = 10, UserId = anUserUserId },
                    new Log { IsReturned = false, BookId = 4, UserId = anAdminUserId }
                );
                context.SaveChanges();
            }
        }
    }

}
