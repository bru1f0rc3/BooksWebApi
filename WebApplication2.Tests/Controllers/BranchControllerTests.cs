using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers.Book;
using WebApplication2.DTO.Branch;
using WebApplication2.Interfaces;

namespace WebApplication2.Tests.Controllers
{
    public class BranchControllerTests
    {
        private readonly Mock<IBranchService> _mockBranchService;
        private readonly BranchController _controller;

        public BranchControllerTests()
        {
            _mockBranchService = new Mock<IBranchService>();
            _controller = new BranchController(_mockBranchService.Object);
        }

        [Fact]
        public async Task GetAllBranches_ReturnsOkWithBranchList()
        {
            // Arrange
            var branches = new List<BranchDTO>
            {
                new BranchDTO { id = 1, name = "Центральная библиотека" },
                new BranchDTO { id = 2, name = "Филиал №1" }
            };
            _mockBranchService.Setup(s => s.GetAllBranches()).ReturnsAsync(branches);

            // Act
            var result = await _controller.GetAllBranches();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<BranchDTO>>().Subject;
            returnValue.Should().HaveCount(2);
            returnValue[0].name.Should().Be("Центральная библиотека");
        }

        [Fact]
        public async Task GetBranchById_WithValidId_ReturnsOkWithBranch()
        {
            // Arrange
            var branch = new BranchDTO { id = 1, name = "Детская библиотека" };
            _mockBranchService.Setup(s => s.GetBranchById(1)).ReturnsAsync(branch);

            // Act
            var result = await _controller.GetBranchById(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<BranchDTO>().Subject;
            returnValue.name.Should().Be("Детская библиотека");
        }

        [Fact]
        public async Task GetBranchById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockBranchService.Setup(s => s.GetBranchById(999)).ReturnsAsync((BranchDTO)null);

            // Act
            var result = await _controller.GetBranchById(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateBranch_WithValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateBranchDTO { name = "Научная библиотека" };
            var createdBranch = new BranchDTO { id = 1, name = "Научная библиотека" };
            _mockBranchService.Setup(s => s.CreateBranch(createDto)).ReturnsAsync(createdBranch);

            // Act
            var result = await _controller.CreateBranch(createDto);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(BranchController.GetBranchById));
            var returnValue = createdResult.Value.Should().BeAssignableTo<BranchDTO>().Subject;
            returnValue.name.Should().Be("Научная библиотека");
        }

        [Fact]
        public async Task UpdateBranch_WithValidData_ReturnsOkWithUpdatedBranch()
        {
            // Arrange
            var updateDto = new UpdateBranchDTO { name = "Городская библиотека" };
            var updatedBranch = new BranchDTO { id = 1, name = "Городская библиотека" };
            _mockBranchService.Setup(s => s.UpdateBranch(1, updateDto)).ReturnsAsync(updatedBranch);

            // Act
            var result = await _controller.UpdateBranch(1, updateDto);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<BranchDTO>().Subject;
            returnValue.name.Should().Be("Городская библиотека");
        }

        [Fact]
        public async Task UpdateBranch_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateBranchDTO { name = "Городская библиотека" };
            _mockBranchService.Setup(s => s.UpdateBranch(999, updateDto)).ReturnsAsync((BranchDTO)null);

            // Act
            var result = await _controller.UpdateBranch(999, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteBranch_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _mockBranchService.Setup(s => s.DeleteBranch(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteBranch(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteBranch_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockBranchService.Setup(s => s.DeleteBranch(999)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteBranch(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
