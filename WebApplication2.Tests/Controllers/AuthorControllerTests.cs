using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers.Book;
using WebApplication2.DTO.Author;
using WebApplication2.Interfaces;

namespace WebApplication2.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly Mock<IAuthorService> _mockAuthorService;
        private readonly AuthorController _controller;

        public AuthorControllerTests()
        {
            _mockAuthorService = new Mock<IAuthorService>();
            _controller = new AuthorController(_mockAuthorService.Object);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsOkWithAuthorList()
        {
            // Arrange
            var authors = new List<AuthorDTO>
            {
                new AuthorDTO { id = 1, full_name = "Александр Пушкин" },
                new AuthorDTO { id = 2, full_name = "Лев Толстой" }
            };
            _mockAuthorService.Setup(s => s.GetAllAuthors()).ReturnsAsync(authors);

            // Act
            var result = await _controller.GetAllAuthors();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<AuthorDTO>>().Subject;
            returnValue.Should().HaveCount(2);
            returnValue[0].full_name.Should().Be("Александр Пушкин");
        }

        [Fact]
        public async Task GetAuthorById_WithValidId_ReturnsOkWithAuthor()
        {
            // Arrange
            var author = new AuthorDTO { id = 1, full_name = "Федор Достоевский" };
            _mockAuthorService.Setup(s => s.GetAuthorById(1)).ReturnsAsync(author);

            // Act
            var result = await _controller.GetAuthorById(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<AuthorDTO>().Subject;
            returnValue.full_name.Should().Be("Федор Достоевский");
        }

        [Fact]
        public async Task GetAuthorById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockAuthorService.Setup(s => s.GetAuthorById(999)).ReturnsAsync((AuthorDTO)null);

            // Act
            var result = await _controller.GetAuthorById(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateAuthor_WithValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateAuthorDTO { full_name = "Антон Чехов" };
            var createdAuthor = new AuthorDTO { id = 1, full_name = "Антон Чехов" };
            _mockAuthorService.Setup(s => s.CreateAuthor(createDto)).ReturnsAsync(createdAuthor);

            // Act
            var result = await _controller.CreateAuthor(createDto);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(AuthorController.GetAuthorById));
            var returnValue = createdResult.Value.Should().BeAssignableTo<AuthorDTO>().Subject;
            returnValue.full_name.Should().Be("Антон Чехов");
        }

        [Fact]
        public async Task UpdateAuthor_WithValidData_ReturnsOkWithUpdatedAuthor()
        {
            // Arrange
            var updateDto = new UpdateAuthorDTO { full_name = "Иван Тургенев" };
            var updatedAuthor = new AuthorDTO { id = 1, full_name = "Иван Тургенев" };
            _mockAuthorService.Setup(s => s.UpdateAuthor(1, updateDto)).ReturnsAsync(updatedAuthor);

            // Act
            var result = await _controller.UpdateAuthor(1, updateDto);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<AuthorDTO>().Subject;
            returnValue.full_name.Should().Be("Иван Тургенев");
        }

        [Fact]
        public async Task UpdateAuthor_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateAuthorDTO { full_name = "Иван Тургенев" };
            _mockAuthorService.Setup(s => s.UpdateAuthor(999, updateDto)).ReturnsAsync((AuthorDTO)null);

            // Act
            var result = await _controller.UpdateAuthor(999, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteAuthor_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _mockAuthorService.Setup(s => s.DeleteAuthor(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAuthor(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAuthor_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockAuthorService.Setup(s => s.DeleteAuthor(999)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAuthor(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
