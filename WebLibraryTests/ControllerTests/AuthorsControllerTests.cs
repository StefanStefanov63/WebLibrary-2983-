using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using WebLibrary.Controllers;
using WebLibrary.Models.Services.Classes;
using WebLibrary.Models.Services.ViewModelsClasses;
using WebLibrary.Data;
using Microsoft.EntityFrameworkCore;
using WebLibrary.Models.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using NuGet.ContentModel;
using System.Security.Policy;
using WebLibrary.Models.Services.Interfaces;

namespace WebLibraryTests.ControllerTests
{
	[TestFixture]
	public class AuthorsControllerTests
	{
		private Mock<IAuthorServices> _mockAuthorServices;
		private ApplicationDbContext _context;
		private AuthorsController _controller;

		[SetUp]
		public void SetUp()
		{
			// Set up an in-memory database
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase(databaseName: "TestDb") // In-memory DB name
							.Options;

			// Create an instance of ApplicationDbContext using the in-memory database
			_context = new ApplicationDbContext(options);

			// Ensure the context is empty before each test (clear any previous data)
			_context.Authors.RemoveRange(_context.Authors);
			_context.Books.RemoveRange(_context.Books);
			_context.SaveChanges();

			// Mock IAuthorServices
			_mockAuthorServices = new Mock<IAuthorServices>();

			// Initialize the controller with the real ApplicationDbContext
			_controller = new AuthorsController(_context); // Pass the in-memory ApplicationDbContext
		}

		#region Test GetAllAuthors Action

		[Test]
		public void GetAllAuthors_Returns_ViewResult_With_Authors()
		{
			// Arrange
			var authors = new List<Author>
			{
				new Author { Id = 1, Name = "Author 1" },
				new Author { Id = 2, Name = "Author 2" }
			};

			// Add authors to the in-memory database
			_context.Authors.AddRange(authors);
			_context.SaveChanges();

			// Setup the mock service to return authors
			_mockAuthorServices.Setup(service => service.GetAllAuthors())
				.Returns(authors.Select(a => new AuthorViewModel { Id = a.Id, Name = a.Name }).ToList());

			// Act
			var result = _controller.GetAllAuthors() as ViewResult;
			var model = result?.Model as ICollection<AuthorViewModel>;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Has.Count.EqualTo(2));
			Assert.That(model.First().Name, Is.EqualTo("Author 1"));
		}

		#endregion

		#region Test RegisterNewAuthor Action

		[Test]
		public void RegisterNewAuthor_Returns_ViewResult_With_Success()
		{
			// Arrange
			string newAuthorName = "New Author";
			_mockAuthorServices.Setup(service => service.RegisterNewAuthor(It.IsAny<string>())).Returns(1); // Success case

			// Act
			var result = _controller.RegisterNewAuthor(newAuthorName) as ViewResult;
			var model = result?.Model;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Is.EqualTo(1)); // Success case (1)
		}

		[Test]
		public void RegisterNewAuthor_Returns_ViewResult_With_Error()
		{
			// Arrange
			string existingAuthorName = "Existing Author";

			// Add an existing author to the in-memory database
			var existingAuthor = new Author { Name = existingAuthorName };
			_context.Authors.Add(existingAuthor);
			_context.SaveChanges();

			// Setup mock service for RegisterNewAuthor to return 2 (error, author already exists)
			_mockAuthorServices.Setup(service => service.RegisterNewAuthor(It.IsAny<string>())).Returns(2); // Error case (author already exists)

			// Act
			var result = _controller.RegisterNewAuthor(existingAuthorName) as ViewResult;
			var model = result?.Model;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Is.EqualTo(2)); // Expecting error case (2) since author already exists
		}

		#endregion

		#region Test DeleteAuthorByName Action

		[Test]
		public void DeleteAuthorByName_Returns_ViewResult_With_Success()
		{
			// Arrange
			string authorNameToDelete = "Author to Delete";

			// Add an author to the in-memory database
			var author = new Author { Name = authorNameToDelete };
			_context.Authors.Add(author);
			_context.SaveChanges();

			// Setup mock service for DeleteAuthorByName to return 2 (success)
			_mockAuthorServices.Setup(service => service.DeleteAuthorByName(It.IsAny<string>())).Returns(2); // Success case (no books)

			// Act
			var result = _controller.DeleteAuthorByName(authorNameToDelete) as ViewResult;
			var model = result?.Model;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Is.EqualTo(2)); // Expecting success (2)
		}

		[Test]
		public void DeleteAuthorByName_Returns_ViewResult_With_Error_Book_Associated()
		{
			// Arrange
			string authorNameWithBooks = "Author With Books";

			// Add an author to the in-memory database
			var authorWithBooks = new Author { Name = authorNameWithBooks };
			_context.Authors.Add(authorWithBooks);

			// Add a book associated with this author
			var book = new Book { AuthorId = authorWithBooks.Id, Title = "Book 1" };
			_context.Books.Add(book);
			_context.SaveChanges();

			// Setup mock service for DeleteAuthorByName to return 1 (error, author has books)
			_mockAuthorServices.Setup(service => service.DeleteAuthorByName(It.IsAny<string>())).Returns(1); // Error case (author has books)

			// Act
			var result = _controller.DeleteAuthorByName(authorNameWithBooks) as ViewResult;
			var model = result?.Model;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Is.EqualTo(1)); // Expecting error (1) because the author has books
		}

		[Test]
		public void DeleteAuthorByName_Returns_ViewResult_With_Author_Not_Found()
		{
			// Arrange
			string nonExistingAuthor = "Non Existing Author";
			_mockAuthorServices.Setup(service => service.DeleteAuthorByName(It.IsAny<string>())).Returns(3); // Error case (author not found)

			// Act
			var result = _controller.DeleteAuthorByName(nonExistingAuthor) as ViewResult;
			var model = result?.Model;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Is.EqualTo(3)); // Error case (3)
		}

		#endregion

		#region Test UpdateAuthorName Action

		[Test]
		public void UpdateAuthorName_Returns_ViewResult_With_Success()
		{
			// Arrange
			string oldAuthorName = "Old Author Name";
			string newAuthorName = "New Author Name";

			// Add the author with the old name to the in-memory database
			var author = new Author { Name = oldAuthorName };
			_context.Authors.Add(author);
			_context.SaveChanges();

			// Setup mock service for UpdateAuthorName to return 1 (success)
			_mockAuthorServices.Setup(service => service.UpdateAuthorName(It.IsAny<string>(), It.IsAny<string>())).Returns(1); // Success case

			// Act
			var result = _controller.UpdateAuthorName(oldAuthorName, newAuthorName) as ViewResult;
			var model = result?.Model;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Is.EqualTo(1)); // Expecting success (1) because the author was found and updated
		}

		[Test]
		public void UpdateAuthorName_Returns_ViewResult_With_Error_Author_Not_Found()
		{
			// Arrange
			string nonExistingAuthor = "Non Existing Author";
			string newAuthorName = "New Author Name";
			_mockAuthorServices.Setup(service => service.UpdateAuthorName(It.IsAny<string>(), It.IsAny<string>())).Returns(2); // Error case (author not found)

			// Act
			var result = _controller.UpdateAuthorName(nonExistingAuthor, newAuthorName) as ViewResult;
			var model = result?.Model;

			// Assert
			Assert.That(result, Is.InstanceOf<ViewResult>());
			Assert.That(model, Is.EqualTo(2)); // Error case (2)
		}

		#endregion
		[TearDown]
		public void TearDown()
		{
			// Dispose of the controller and mock objects if necessary
			_controller?.Dispose();
			_context.Dispose(); 
		}
	}
}