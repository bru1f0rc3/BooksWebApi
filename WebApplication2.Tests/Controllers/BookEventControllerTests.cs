using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers.BookEvent;
using WebApplication2.DTO.BookEvent;
using WebApplication2.Interfaces;

namespace WebApplication2.Tests.Controllers
{
    public class BookEventControllerTests
    {
        private readonly Mock<IBookEventService> _mockBookEventService;
        private readonly BookEventController _controller;

        public BookEventControllerTests()
        {
            _mockBookEventService = new Mock<IBookEventService>();
            _controller = new BookEventController(_mockBookEventService.Object);
        }

        [Fact]
        public async Task GetBookEvents_ReturnsOkWithEventList()
        {
            // Arrange
            var events = new List<BookEventListDTO>
            {
                new BookEventListDTO { bookevent_id = 1, book_id = 1, account_id = 1 },
                new BookEventListDTO { bookevent_id = 2, book_id = 2, account_id = 2 }
            };
            _mockBookEventService.Setup(s => s.GetBookEvents()).ReturnsAsync(events);

            // Act
            var result = await _controller.GetBookEvents();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<BookEventListDTO>>().Subject;
            returnValue.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetActiveRequests_ReturnsOkWithRequestList()
        {
            // Arrange
            var requests = new List<BookEventListDTO>
            {
                new BookEventListDTO { bookevent_id = 1, book_title = "Война и мир", user_name = "Иван Иванов" }
            };
            _mockBookEventService.Setup(s => s.GetActiveRequests()).ReturnsAsync(requests);

            // Act
            var result = await _controller.GetActiveRequests();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<BookEventListDTO>>().Subject;
            returnValue.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserBookHistory_ReturnsOkWithHistoryList()
        {
            // Arrange
            var history = new List<UserBookHistoryDTO>
            {
                new UserBookHistoryDTO { book_id = 1 }
            };
            _mockBookEventService.Setup(s => s.GetUserBookHistory(1)).ReturnsAsync(history);

            // Act
            var result = await _controller.GetUserBookHistory(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<UserBookHistoryDTO>>().Subject;
            returnValue.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserRequestedBooks_ReturnsOkWithBookList()
        {
            // Arrange
            var books = new List<UserBookEventDTO>
            {
                new UserBookEventDTO { book_id = 1, book_title = "Преступление и наказание" }
            };
            _mockBookEventService.Setup(s => s.GetUserBooksByEventType(1, 1)).ReturnsAsync(books);

            // Act
            var result = await _controller.GetUserRequestedBooks(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<UserBookEventDTO>>().Subject;
            returnValue.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserReturnedBooks_ReturnsOkWithBookList()
        {
            // Arrange
            var books = new List<UserBookEventDTO>
            {
                new UserBookEventDTO { book_id = 1, book_title = "Анна Каренина" }
            };
            _mockBookEventService.Setup(s => s.GetUserBooksByEventType(1, 2)).ReturnsAsync(books);

            // Act
            var result = await _controller.GetUserReturnedBooks(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<UserBookEventDTO>>().Subject;
            returnValue.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserTakedBooks_ReturnsOkWithBookList()
        {
            // Arrange
            var books = new List<UserBookEventDTO>
            {
                new UserBookEventDTO { book_id = 1, book_title = "Мастер и Маргарита" }
            };
            _mockBookEventService.Setup(s => s.GetUserBooksByEventType(1, 3)).ReturnsAsync(books);

            // Act
            var result = await _controller.GetUserTakedBooks(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<UserBookEventDTO>>().Subject;
            returnValue.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserSavedBooks_ReturnsOkWithBookList()
        {
            // Arrange
            var books = new List<UserBookEventDTO>
            {
                new UserBookEventDTO { book_id = 1, book_title = "Евгений Онегин" }
            };
            _mockBookEventService.Setup(s => s.GetUserBooksByEventType(1, 4)).ReturnsAsync(books);

            // Act
            var result = await _controller.GetUserSavedBooks(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<UserBookEventDTO>>().Subject;
            returnValue.Should().HaveCount(1);
        }

        [Fact]
        public async Task RequestBook_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.RequestBook(1, 1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RequestBook(1, 1);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mockBookEventService.Verify(s => s.RequestBook(1, 1), Times.Once);
        }

        [Fact]
        public async Task AcceptRequest_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.AcceptRequest(1, 1)).ReturnsAsync(true);

            // Act
            var result = await _controller.AcceptRequest(1, 1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be("Запрос успешно принят");
        }

        [Fact]
        public async Task AcceptRequest_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.AcceptRequest(999, 1)).ReturnsAsync(false);

            // Act
            var result = await _controller.AcceptRequest(999, 1);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Запрос не найден или уже обработан");
        }

        [Fact]
        public async Task RejectRequest_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.RejectRequest(1, 1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RejectRequest(1, 1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be("Запрос отклонен");
        }

        [Fact]
        public async Task SaveBook_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.SaveBook(1, 1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SaveBook(1, 1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task TakeBook_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.TakeBook(1, 1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.TakeBook(1, 1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ReturnBook_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.ReturnBook(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ReturnBook(1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RemoveSavedBook_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.RemoveSavedBook(1, 1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveSavedBook(1, 1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task CancelRequest_WithValidData_ReturnsOk()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.CancelRequest(1, 1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelRequest(1, 1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task GetTakedBooks_ReturnsOkWithBookList()
        {
            // Arrange
            var books = new List<BookEventListDTO>
            {
                new BookEventListDTO { bookevent_id = 1, book_title = "Идиот" }
            };
            _mockBookEventService.Setup(s => s.GetAllTakedBooks()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetTakedBooks();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<BookEventListDTO>>().Subject;
            returnValue.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetRequestDetail_WithValidId_ReturnsOkWithDetail()
        {
            // Arrange
            var detail = new BookRequestDetailDTO
            {
                event_id = 1,
                book_title = "Братья Карамазовы"
            };
            _mockBookEventService.Setup(s => s.GetRequestDetail(1)).ReturnsAsync(detail);

            // Act
            var result = await _controller.GetRequestDetail(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<BookRequestDetailDTO>().Subject;
            returnValue.book_title.Should().Be("Братья Карамазовы");
        }

        [Fact]
        public async Task GetRequestDetail_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockBookEventService.Setup(s => s.GetRequestDetail(999))
                .ThrowsAsync(new Exception("Request not found"));

            // Act
            var result = await _controller.GetRequestDetail(999);

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be("Request not found");
        }
    }
}
