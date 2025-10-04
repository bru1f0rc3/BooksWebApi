using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers.Book;
using WebApplication2.Services.Book;
using WebApplication2.DTO.Book;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksWebApi.Tests.Controllers
{
    public class BookControllerTests
    {
        private readonly Mock<BookService> _mockBookService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockBookService = new Mock<BookService>();
            _controller = new BookController(_mockBookService.Object);
        }

        [Fact]
        public async Task GetBooks_ReturnsOkResult_WithListOfBooks()
        {
            // Arrange
            var expectedBooks = new List<BookListDto>
            {
                new BookListDto
                {
                    id = 1,
                    title = "Test Book 1",
                    description = "Description 1",
                    author_name = "Author 1",
                    category_name = "Category 1",
                    branch_name = "Branch 1"
                },
                new BookListDto
                {
                    id = 2,
                    title = "Test Book 2",
                    description = "Description 2",
                    author_name = "Author 2",
                    category_name = "Category 2",
                    branch_name = "Branch 2"
                }
            };

            _mockBookService
                .Setup(service => service.BookListedGet())
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<List<BookListDto>>(okResult.Value);
            Assert.Equal(2, returnedBooks.Count);
            Assert.Equal("Test Book 1", returnedBooks[0].title);
        }

        [Fact]
        public async Task AddBook_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var addBook = new AddBook
            {
                Title = "New Book",
                Description = "New Description",
                Fragment = "New Fragment",
                AuthorId = 1,
                CategoryId = 1,
                BranchId = 1
            };

            _mockBookService
                .Setup(service => service.AddBook(It.IsAny<AddBook>(), It.IsAny<IFormFile>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddBook(addBook, null);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockBookService.Verify(service => service.AddBook(addBook, null), Times.Once);
        }

        [Fact]
        public async Task AddBook_WithException_ReturnsBadRequest()
        {
            // Arrange
            var addBook = new AddBook
            {
                Title = "New Book",
                Description = "New Description"
            };

            var errorMessage = "Database connection failed";
            _mockBookService
                .Setup(service => service.AddBook(It.IsAny<AddBook>(), It.IsAny<IFormFile>()))
                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.AddBook(addBook, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task EditBook_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var editBook = new EditBook
            {
                Id = 1,
                Title = "Updated Book",
                Description = "Updated Description",
                Fragment = "Updated Fragment",
                AuthorId = 1,
                CategoryId = 1,
                BranchId = 1
            };

            _mockBookService
                .Setup(service => service.EditBook(It.IsAny<EditBook>(), It.IsAny<IFormFile>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditBook(editBook, null);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockBookService.Verify(service => service.EditBook(editBook, null), Times.Once);
        }

        [Fact]
        public async Task RemoveBook_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int bookId = 1;

            _mockBookService
                .Setup(service => service.RemoveBook(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveBook(bookId);

            // Assert
            Assert.IsType<OkResult>(result.Result);
            _mockBookService.Verify(service => service.RemoveBook(bookId), Times.Once);
        }

        [Fact]
        public async Task SearchBooks_WithSearchTerm_ReturnsFilteredBooks()
        {
            // Arrange
            var searchTerm = "test";
            var expectedBooks = new List<BookListDto>
            {
                new BookListDto
                {
                    id = 1,
                    title = "Test Book",
                    description = "Test Description",
                    author_name = "Test Author"
                }
            };

            _mockBookService
                .Setup(service => service.SearchBooks(searchTerm, null))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.SearchBooks(searchTerm, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<List<BookListDto>>(okResult.Value);
            Assert.Single(returnedBooks);
            Assert.Contains("Test", returnedBooks[0].title);
        }

        [Fact]
        public async Task GetBookDetail_WithValidId_ReturnsBookDetail()
        {
            // Arrange
            int bookId = 1;
            var expectedBook = new BookDetailDto
            {
                id = bookId,
                title = "Test Book",
                description = "Test Description",
                author_name = "Test Author",
                category_name = "Test Category",
                branch_name = "Test Branch"
            };

            _mockBookService
                .Setup(service => service.GetBookDetail(bookId))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _controller.GetBookDetail(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBook = Assert.IsType<BookDetailDto>(okResult.Value);
            Assert.Equal(bookId, returnedBook.id);
            Assert.Equal("Test Book", returnedBook.title);
        }

        [Fact]
        public async Task GetBookDetail_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int bookId = 999;
            var errorMessage = "Книга не найдена";

            _mockBookService
                .Setup(service => service.GetBookDetail(bookId))
                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetBookDetail(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(errorMessage, notFoundResult.Value);
        }
    }
}
