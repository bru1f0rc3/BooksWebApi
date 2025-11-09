using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers.Book;
using WebApplication2.DTO.Category;
using WebApplication2.Interfaces;

namespace WebApplication2.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkWithCategoryList()
        {
            // Arrange
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO { id = 1, name = "Фантастика" },
                new CategoryDTO { id = 2, name = "Детективы" }
            };
            _mockCategoryService.Setup(s => s.GetAllCategories()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<CategoryDTO>>().Subject;
            returnValue.Should().HaveCount(2);
            returnValue[0].name.Should().Be("Фантастика");
        }

        [Fact]
        public async Task GetCategoryById_WithValidId_ReturnsOkWithCategory()
        {
            // Arrange
            var category = new CategoryDTO { id = 1, name = "Классика" };
            _mockCategoryService.Setup(s => s.GetCategoryById(1)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategoryById(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<CategoryDTO>().Subject;
            returnValue.name.Should().Be("Классика");
        }

        [Fact]
        public async Task GetCategoryById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.GetCategoryById(999)).ReturnsAsync((CategoryDTO)null);

            // Act
            var result = await _controller.GetCategoryById(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateCategory_WithValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateCategoryDTO { name = "Научная литература" };
            var createdCategory = new CategoryDTO { id = 1, name = "Научная литература" };
            _mockCategoryService.Setup(s => s.CreateCategory(createDto)).ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.CreateCategory(createDto);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(CategoryController.GetCategoryById));
            var returnValue = createdResult.Value.Should().BeAssignableTo<CategoryDTO>().Subject;
            returnValue.name.Should().Be("Научная литература");
        }

        [Fact]
        public async Task UpdateCategory_WithValidData_ReturnsOkWithUpdatedCategory()
        {
            // Arrange
            var updateDto = new UpdateCategoryDTO { name = "Приключения" };
            var updatedCategory = new CategoryDTO { id = 1, name = "Приключения" };
            _mockCategoryService.Setup(s => s.UpdateCategory(1, updateDto)).ReturnsAsync(updatedCategory);

            // Act
            var result = await _controller.UpdateCategory(1, updateDto);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<CategoryDTO>().Subject;
            returnValue.name.Should().Be("Приключения");
        }

        [Fact]
        public async Task UpdateCategory_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateCategoryDTO { name = "Приключения" };
            _mockCategoryService.Setup(s => s.UpdateCategory(999, updateDto)).ReturnsAsync((CategoryDTO)null);

            // Act
            var result = await _controller.UpdateCategory(999, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteCategory_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.DeleteCategory(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteCategory_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.DeleteCategory(999)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCategory(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
