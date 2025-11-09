using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication2.Controllers.Dashboard;
using WebApplication2.DTO.Auth;
using WebApplication2.Interfaces;
using Xunit;

namespace WebApplication2.Tests.Controllers
{
    /// <summary>
    /// Тесты для контроллера аутентификации
    /// </summary>
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        #region Login Tests

        /// <summary>
        /// Тест: Login должен возвращать токен при успешной аутентификации
        /// </summary>
        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Login = "testuser",
                Password = "password123"
            };

            var expectedToken = new TokenDTO
            {
                Token = "jwt-token-here",
                Role = "Admin",
                UserId = 1,
                FullName = "Тестовый Пользователь"
            };

            _mockAuthService
                .Setup(s => s.Login(It.IsAny<LoginDTO>()))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var token = okResult.Value.Should().BeAssignableTo<TokenDTO>().Subject;
            token.Token.Should().Be(expectedToken.Token);
            token.Role.Should().Be(expectedToken.Role);
            token.UserId.Should().Be(expectedToken.UserId);
            token.FullName.Should().Be(expectedToken.FullName);

            _mockAuthService.Verify(s => s.Login(It.IsAny<LoginDTO>()), Times.Once);
        }

        /// <summary>
        /// Тест: Login должен возвращать Unauthorized при неверных учетных данных
        /// </summary>
        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Login = "wronguser",
                Password = "wrongpassword"
            };

            _mockAuthService
                .Setup(s => s.Login(It.IsAny<LoginDTO>()))
                .ReturnsAsync((TokenDTO?)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            var unauthorizedResult = result.Result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
            unauthorizedResult.Value.Should().Be("Неверный логин или пароль");

            _mockAuthService.Verify(s => s.Login(It.IsAny<LoginDTO>()), Times.Once);
        }

        /// <summary>
        /// Тест: Login должен возвращать токен с правильной ролью для обычного пользователя
        /// </summary>
        [Fact]
        public async Task Login_ShouldReturnToken_WithUserRole()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Login = "user",
                Password = "userpass"
            };

            var expectedToken = new TokenDTO
            {
                Token = "user-jwt-token",
                Role = "User",
                UserId = 2,
                FullName = "Обычный Пользователь"
            };

            _mockAuthService
                .Setup(s => s.Login(It.IsAny<LoginDTO>()))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var token = okResult.Value.Should().BeAssignableTo<TokenDTO>().Subject;
            token.Role.Should().Be("User");
        }

        /// <summary>
        /// Тест: Login должен вызывать сервис с переданными учетными данными
        /// </summary>
        [Fact]
        public async Task Login_ShouldCallAuthService_WithProvidedCredentials()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Login = "specificuser",
                Password = "specificpass"
            };

            _mockAuthService
                .Setup(s => s.Login(loginDto))
                .ReturnsAsync(new TokenDTO
                {
                    Token = "token",
                    Role = "Admin",
                    UserId = 1,
                    FullName = "Test"
                });

            // Act
            await _controller.Login(loginDto);

            // Assert
            _mockAuthService.Verify(s => s.Login(It.Is<LoginDTO>(dto =>
                dto.Login == "specificuser" &&
                dto.Password == "specificpass"
            )), Times.Once);
        }

        #endregion
    }
}
