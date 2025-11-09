using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers.Dashboard;
using WebApplication2.DTO.Dashboard;
using WebApplication2.Interfaces;

namespace WebApplication2.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task CreateUser_WithValidData_ReturnsOk()
        {
            // Arrange
            var createDto = new CreateAccountDTO
            {
                login = "testuser",
                password = "password123",
                full_name = "Иван Иванов",
                phone = "+79991234567",
                email = "test@test.com"
            };
            _mockUserService.Setup(s => s.CreateUserAsync(createDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateUser(createDto);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockUserService.Verify(s => s.CreateUserAsync(createDto), Times.Once);
        }

        [Fact]
        public async Task ChangeEmail_WithValidData_ReturnsOk()
        {
            // Arrange
            var changeEmailDto = new ChangeEmailUserDTO
            {
                id = 1,
                email = "newemail@test.com"
            };
            _mockUserService.Setup(s => s.UpdateEmailUser(changeEmailDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.ChangeEmail(changeEmailDto);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockUserService.Verify(s => s.UpdateEmailUser(changeEmailDto), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_WithValidData_ReturnsOk()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDTO
            {
                id = 1,
                old_password = "oldpass123",
                new_password = "newpass123"
            };
            _mockUserService.Setup(s => s.UpdatePasswordUser(changePasswordDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.ChangePassword(changePasswordDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be("Пароль успешно изменен");
        }

        [Fact]
        public async Task ChangePassword_WithInvalidOldPassword_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDTO
            {
                id = 1,
                old_password = "wrongpass",
                new_password = "newpass123"
            };
            _mockUserService.Setup(s => s.UpdatePasswordUser(changePasswordDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.ChangePassword(changePasswordDto);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Неверный старый пароль");
        }

        [Fact]
        public async Task ChangePassword_WithEmptyOldPassword_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDTO
            {
                id = 1,
                old_password = "",
                new_password = "newpass123"
            };

            // Act
            var result = await _controller.ChangePassword(changePasswordDto);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Старый и новый пароли обязательны");
        }

        [Fact]
        public async Task ChangePassword_WithEmptyNewPassword_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDTO
            {
                id = 1,
                old_password = "oldpass123",
                new_password = ""
            };

            // Act
            var result = await _controller.ChangePassword(changePasswordDto);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Старый и новый пароли обязательны");
        }

        [Fact]
        public async Task ChangePassword_WithNullPasswords_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDTO
            {
                id = 1,
                old_password = null!,
                new_password = null!
            };

            // Act
            var result = await _controller.ChangePassword(changePasswordDto);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Старый и новый пароли обязательны");
        }
    }
}
