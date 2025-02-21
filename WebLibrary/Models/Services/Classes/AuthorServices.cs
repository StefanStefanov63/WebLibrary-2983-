using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary.Data;
using WebLibrary.Models.Services.ViewModelsClasses;
using WebLibrary.Models.Services.Interfaces;
using WebLibrary.Models.Entities;

namespace WebLibrary.Models.Services.Classes
{
    public class AuthorServices : IAuthorServices
    {
        private readonly ApplicationDbContext db;

        public AuthorServices(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int DeleteAuthorByName(string aName)
        {
            var anAuthor = db.Authors.FirstOrDefault(x => x.Name.Trim() == aName.Trim());
            if (anAuthor is not null)
            {
                var aBook = db.Books.FirstOrDefault(x => x.AuthorId == anAuthor.Id);
                if (aBook is not null)
                {
                    return 1;
                }
                else
                {
                    db.Authors.Remove(anAuthor);
                    db.SaveChanges();
                    return 2;
                }
            }
            else
            {
                return 3;
            }
            
        }

        public ICollection<AuthorViewModel> GetAllAuthors()
        {
            var anAuthors = db.Authors.ToList();
            var allAuthors = new List<AuthorViewModel>();
            foreach (var Author in anAuthors)
            {
                AuthorViewModel anAuthorViewModel = new AuthorViewModel();
                anAuthorViewModel.Id = Author.Id;
                anAuthorViewModel.Name = Author.Name;
                allAuthors.Add(anAuthorViewModel);
            }
            return allAuthors;
        }

        public int RegisterNewAuthor(string aName)
        {
            var anAuthor = db.Authors.FirstOrDefault(x => x.Name.Trim() == aName.Trim());
            if (anAuthor is null)
            {
                Author author = new Author();
                author.Name = aName.Trim();
                db.Authors.Add(author);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public int UpdateAuthorName(string aOldName, string aNewName)
        {
            var anAuthor = db.Authors.FirstOrDefault(x => x.Name.Trim() == aOldName.Trim());
            if (anAuthor is not null)
            {
                anAuthor.Name = aNewName.Trim();
                db.SaveChanges();
                return 1;
            }
            else 
            {
                return 2;
            }
            
        }
    }
}
