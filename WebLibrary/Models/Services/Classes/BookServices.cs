using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary.Models.Services.ViewModelsClasses;
using WebLibrary.Models.Services.Interfaces;
using WebLibrary.Models.Entities;
using WebLibrary.Data;
using System.Diagnostics.CodeAnalysis;

namespace WebLibrary.Models.Services.Classes
{
    public class BookServices : IBookServices
    {
        private readonly ApplicationDbContext db;

        public BookServices(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int DeleteBookByTitle(string aTitle)
        {
            var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aTitle.Trim());
            if (aBook != null)
            {
                var alog = db.Logs.FirstOrDefault(x => x.BookId == aBook.Id && x.IsReturned == false);
                if (alog is null)
                {
                    db.Books.Remove(aBook);
                    db.SaveChanges();
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 3;
            }
            
        }

        public ICollection<BookViewModel> GetAllBookFromAuthor(string amAuthor)
        {
            var aBooks = db.Books.ToList();
            var allBooks = new List<BookViewModel>();
            foreach (var aBook in aBooks)
            {
                string anAuthorName = db.Authors.FirstOrDefault(x => x.Id == aBook.AuthorId).Name;
                if (anAuthorName == amAuthor)
                {
                    BookViewModel aBookViewModel = new BookViewModel();
                    aBookViewModel.Id = aBook.Id;
                    aBookViewModel.Title = aBook.Title;
                    aBookViewModel.Description = aBook.Description;
                    aBookViewModel.Quantity = aBook.Quantity;
                    aBookViewModel.Author = anAuthorName;
                    allBooks.Add(aBookViewModel);
                }
            }
            return allBooks;
        }

        public ICollection<BookViewModel> GetAllBooks()
        {
            var aBooks = db.Books.ToList();
            var allBooks = new List<BookViewModel>();
            foreach (var aBook in aBooks)
            {
                BookViewModel aBookViewModel = new BookViewModel();
                aBookViewModel.Id = aBook.Id;
                aBookViewModel.Title = aBook.Title;
                aBookViewModel.Description = aBook.Description;
                aBookViewModel.Quantity = aBook.Quantity;
                aBookViewModel.Author = db.Authors.FirstOrDefault(x => x.Id == aBook.AuthorId).Name;
                allBooks.Add(aBookViewModel);
            }
            return allBooks;
        }

        public int[] RegisterNewBook(string aTitle, string anAuthorName, string aDescription, int aQuantity)
        {
            var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aTitle.Trim());
            int[] ouptut = { 0,0,0};
            if (aBook is null)
            {
				ouptut[0] = 1;
                Book book = new Book();
                book.Title = aTitle.Trim();
                if (aDescription != null)
                {
                    book.Description = aDescription.Trim();
                }
                if (aQuantity >= 0)
                {
                    book.Quantity = aQuantity;
                }
                else
                {
					ouptut[1] = 2;
                    book.Quantity = 0;
                }
                var anAuthor = db.Authors.FirstOrDefault(x => x.Name.Trim() == anAuthorName.Trim());
                if (anAuthor is null)
                {
					ouptut[2] = 3;
                    anAuthor = new Author { Name = anAuthorName.Trim() };
                    db.Authors.Add(anAuthor);
                    db.SaveChanges();
                    book.AuthorId = db.Authors.FirstOrDefault(x => x.Name.Trim() == anAuthorName.Trim()).Id;
                }
                else
                {
                    book.AuthorId = anAuthor.Id;
                }
                db.Books.Add(book);
                db.SaveChanges();
            }
            else
            {
				ouptut[0] = 4;
            }
            return ouptut;
        }

        public BookViewModel GetBookByTitle(string aTitle)
        {
            var aBookViewModel = new BookViewModel();
            var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aTitle.Trim());
            if (aBook != null)
            {
                aBookViewModel.Id = aBook.Id;
                aBookViewModel.Title = aBook.Title.Trim();
                aBookViewModel.Description = aBook.Description;
                aBookViewModel.Quantity = aBook.Quantity;
                aBookViewModel.Author = db.Authors.FirstOrDefault(x => x.Id == aBook.AuthorId).Name;
                return aBookViewModel;
            }
            return null;
        }

        public int[] UpdateBook(string anOldTitle, string aNewTitle, string aNewAuthor, string aNewDescription, int aNewQuantity)
        {
            int[]ouptut = { 0,0,0,0,0};
            if (db.Books.FirstOrDefault(x => x.Title.Trim() == anOldTitle.Trim()) is not null)
            {
				ouptut[0] = 1;
				ouptut[1] = UpdateBookTitle(anOldTitle, aNewTitle);
                if (ouptut[1] == 0)
                    aNewTitle = anOldTitle;
				ouptut[2] = UpdateBookAuthor(aNewTitle, aNewAuthor);
				ouptut[3] = UpdateBookDescription(aNewTitle, aNewDescription);
				ouptut[4] = UpdateBookQuantity(aNewTitle, aNewQuantity);
            }
            return ouptut;
        }

        public int UpdateBookAuthor(string aTitle, string aNewAuthor)
        {
            
            int output = 0;
            if (aNewAuthor is not null)
            {
                var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aTitle.Trim());
                var anNewAuthor = db.Authors.FirstOrDefault(x => x.Name.Trim() == aNewAuthor.Trim());
                if (anNewAuthor is null)
                {
                    anNewAuthor = new Author { Name = aNewAuthor.Trim() };
                    db.Authors.Add(anNewAuthor);
                    db.SaveChanges();
                    aBook.AuthorId = db.Authors.FirstOrDefault(x => x.Name.Trim() == aNewAuthor.Trim()).Id;
                    output = 1;
                }
                else
                {
                    output = 2;
                    aBook.AuthorId = anNewAuthor.Id;
                }
            }
            db.SaveChanges();
            return output;
            
        }

        public int UpdateBookDescription(string aTitle, string aNewDescription)
        {
            var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aTitle.Trim());
            if(aNewDescription == "save")
            {
                return 0;
            }
            else
            { 
                aBook.Description = aNewDescription;
                db.SaveChanges();
                return 1;
            }
            
            
        }

        public int UpdateBookQuantity(string aTitle, int aNewQuantity)
        {
            var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == aTitle.Trim());
                if (aNewQuantity >= 0)
                {
                    Console.WriteLine($"Book '{aTitle.Trim()}''s Quantity was successfully changed from {aBook.Quantity} to {aNewQuantity}.");
                    aBook.Quantity = aNewQuantity;db.SaveChanges();
                return 1;
                }
                else
                {
                return 0;
                }
            
        }

        public int UpdateBookTitle(string anOldTitle, string aNewTitle)
        {
            
            if (aNewTitle is not null)
            {
                var aBook = db.Books.FirstOrDefault(x => x.Title.Trim() == anOldTitle.Trim());
                aBook.Title = aNewTitle.Trim();
                db.SaveChanges();
                return 1;
            }
            else 
            {
                return 0; 
            }
            
        }
    }
}
