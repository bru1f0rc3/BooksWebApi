using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication2.Controllers.Book;
using WebApplication2.DTO.Book;
using WebApplication2.Interfaces;
using Xunit;

namespace WebApplication2.Tests.Controllers
{
    /// <summary>
    /// Тесты для контроллера управления книгами
    /// </summary>
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BookController(_mockBookService.Object);
        }

        #region GetBooks Tests

        /// <summary>
        /// Тест: GetBooks должен возвращать список всех книг
        /// </summary>
        [Fact]
        public async Task GetBooks_ShouldReturnListOfBooks()
        {
            // Arrange
            var expectedBooks = new List<BookListDTO>
            {
                new BookListDTO
                {
                    id = 1,
                    title = "Тестовая книга 1",
                    author_name = "Автор 1",
                    category_name = "Категория 1",
                    branch_name = "Филиал 1"
                },
                new BookListDTO
                {
                    id = 2,
                    title = "Тестовая книга 2",
                    author_name = "Автор 2",
                    category_name = "Категория 2",
                    branch_name = "Филиал 2"
                }
            };

            _mockBookService
                .Setup(s => s.BookListedGet())
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var books = okResult.Value.Should().BeAssignableTo<List<BookListDTO>>().Subject;
            books.Should().HaveCount(2);
            books.Should().BeEquivalentTo(expectedBooks);

            _mockBookService.Verify(s => s.BookListedGet(), Times.Once);
        }

        /// <summary>
        /// Тест: GetBooks должен возвращать пустой список, если книг нет
        /// </summary>
        [Fact]
        public async Task GetBooks_ShouldReturnEmptyList_WhenNoBooksExist()
        {
            // Arrange
            var emptyList = new List<BookListDTO>();
            _mockBookService
                .Setup(s => s.BookListedGet())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var books = okResult.Value.Should().BeAssignableTo<List<BookListDTO>>().Subject;
            books.Should().BeEmpty();
        }

        #endregion

        #region AddBook Tests

        /// <summary>
        /// Тест: AddBook должен успешно добавлять книгу
        /// </summary>
        [Fact]
        public async Task AddBook_ShouldReturnOk_WhenBookAddedSuccessfully()
        {
            // Arrange
            var book = new AddBook
            {
                Title = "Новая книга",
                Description = "Описание",
                AuthorId = 1,
                CategoryId = 1,
                BranchId = 1
            };

            _mockBookService
                .Setup(s => s.AddBook(It.IsAny<AddBook>(), It.IsAny<IFormFile>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddBook(book, null);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockBookService.Verify(s => s.AddBook(book, null), Times.Once);
        }

        /// <summary>
        /// Тест: AddBook должен возвращать BadRequest при ошибке
        /// </summary>
        [Fact]
        public async Task AddBook_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var book = new AddBook { Title = "Книга" };
            var errorMessage = "Ошибка при добавлении книги";

            _mockBookService
                .Setup(s => s.AddBook(It.IsAny<AddBook>(), It.IsAny<IFormFile>()))
                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.AddBook(book, null);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(errorMessage);
        }

        #endregion

        #region EditBook Tests

        /// <summary>
        /// Тест: EditBook должен успешно редактировать книгу
        /// </summary>
        [Fact]
        public async Task EditBook_ShouldReturnOk_WhenBookEditedSuccessfully()
        {
            // Arrange
            var book = new EditBook
            {
                Id = 1,
                Title = "Обновленная книга",
                Description = "Обновленное описание",
                AuthorId = 1,
                CategoryId = 1,
                BranchId = 1
            };

            _mockBookService
                .Setup(s => s.EditBook(It.IsAny<EditBook>(), It.IsAny<IFormFile>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditBook(book, null);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockBookService.Verify(s => s.EditBook(book, null), Times.Once);
        }

        /// <summary>
        /// Тест: EditBook должен возвращать BadRequest при ошибке
        /// </summary>
        [Fact]
        public async Task EditBook_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var book = new EditBook { Id = 1, Title = "Книга" };
            var errorMessage = "Ошибка при редактировании книги";

            _mockBookService
                .Setup(s => s.EditBook(It.IsAny<EditBook>(), It.IsAny<IFormFile>()))
                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.EditBook(book, null);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(errorMessage);
        }

        #endregion

        #region RemoveBook Tests

        /// <summary>
        /// Тест: RemoveBook должен успешно удалять книгу
        /// </summary>
        [Fact]
        public async Task RemoveBook_ShouldReturnOk_WhenBookRemovedSuccessfully()
        {
            // Arrange
            int bookId = 1;

            _mockBookService
                .Setup(s => s.RemoveBook(bookId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveBook(bookId);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockBookService.Verify(s => s.RemoveBook(bookId), Times.Once);
        }

        #endregion

        #region SearchBooks Tests

        /// <summary>
        /// Тест: SearchBooks должен возвращать найденные книги по поисковому запросу
        /// </summary>
        [Fact]
        public async Task SearchBooks_ShouldReturnFilteredBooks_WhenSearchTermProvided()
        {
            // Arrange
            var searchTerm = "Тест";
            var expectedBooks = new List<BookListDTO>
            {
                new BookListDTO
                {
                    id = 1,
                    title = "Тестовая книга",
                    author_name = "Автор",
                    category_name = "Категория",
                    branch_name = "Филиал"
                }
            };

            _mockBookService
                .Setup(s => s.SearchBooks(searchTerm, null))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.SearchBooks(searchTerm, null);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var books = okResult.Value.Should().BeAssignableTo<List<BookListDTO>>().Subject;
            books.Should().HaveCount(1);
            books[0].title.Should().Contain("Тест");
        }

        /// <summary>
        /// Тест: SearchBooks должен возвращать книги по категории
        /// </summary>
        [Fact]
        public async Task SearchBooks_ShouldReturnFilteredBooks_WhenCategoryIdProvided()
        {
            // Arrange
            int categoryId = 1;
            var expectedBooks = new List<BookListDTO>
            {
                new BookListDTO { id = 1, title = "Книга 1" },
                new BookListDTO { id = 2, title = "Книга 2" }
            };

            _mockBookService
                .Setup(s => s.SearchBooks(null, categoryId))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.SearchBooks(null, categoryId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var books = okResult.Value.Should().BeAssignableTo<List<BookListDTO>>().Subject;
            books.Should().HaveCount(2);
        }

        #endregion

        #region GetBookDetail Tests

        /// <summary>
        /// Тест: GetBookDetail должен возвращать детальную информацию о книге
        /// </summary>
        [Fact]
        public async Task GetBookDetail_ShouldReturnBookDetails_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            var expectedBook = new BookDetailDTO
            {
                id = bookId,
                title = "Детальная книга",
                description = "Полное описание",
                author_name = "Автор",
                category_name = "Категория",
                branch_name = "Филиал"
            };

            _mockBookService
                .Setup(s => s.GetBookDetail(bookId))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _controller.GetBookDetail(bookId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var book = okResult.Value.Should().BeAssignableTo<BookDetailDTO>().Subject;
            book.id.Should().Be(bookId);
            book.title.Should().Be("Детальная книга");
        }

        /// <summary>
        /// Тест: GetBookDetail должен возвращать NotFound, если книга не найдена
        /// </summary>
        [Fact]
        public async Task GetBookDetail_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            int bookId = 999;
            var errorMessage = "Книга не найдена";

            _mockBookService
                .Setup(s => s.GetBookDetail(bookId))
                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetBookDetail(bookId);

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(errorMessage);
        }

        #endregion
    }
}
