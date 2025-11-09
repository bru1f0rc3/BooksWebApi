using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.BookEvent;
using WebApplication2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication2.Controllers.BookEvent
{
    /// <summary>
    /// Контроллер для управления событиями книг (выдача, возврат, бронирование)
    /// </summary>
    [ApiController]
    [Route("api/book-event")]
    public class BookEventController : ControllerBase
    {
        private readonly IBookEventService _bookEventService;

        /// <summary>
        /// Конструктор контроллера событий книг
        /// </summary>
        public BookEventController(IBookEventService bookEventService)
        {
            _bookEventService = bookEventService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<BookEventDTO>>> GetBookEvents()
        {
            var events = await _bookEventService.GetBookEvents();
            return Ok(events);
        }

        [HttpGet("active-requests")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<List<BookEventListDTO>>> GetActiveRequests()
        {
            var requests = await _bookEventService.GetActiveRequests();
            return Ok(requests);
        }

        [HttpGet("user/{accountId}/history")]
        public async Task<ActionResult<List<UserBookHistoryDTO>>> GetUserBookHistory(int accountId)
        {
            var history = await _bookEventService.GetUserBookHistory(accountId);
            return Ok(history);
        }

        [HttpGet("user/{accountId}/requested")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserRequestedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 1);
            return Ok(books);
        }

        [HttpGet("user/{accountId}/returned")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserReturnedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 2);
            return Ok(books);
        }

        [HttpGet("user/{accountId}/taked")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserTakedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 3);
            return Ok(books);
        }

        [HttpGet("user/{accountId}/saved")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserSavedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 4);
            return Ok(books);
        }

        [HttpPost("request")]
        public async Task<ActionResult> RequestBook(int accountId, int bookId)
        {
            await _bookEventService.RequestBook(accountId, bookId);
            return Ok();
        }

        [HttpPost("accept/{eventId}/{librarianId}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> AcceptRequest(int eventId, int librarianId)
        {
            var result = await _bookEventService.AcceptRequest(eventId, librarianId);
            if (!result)
                return BadRequest("Запрос не найден или уже обработан");
            return Ok("Запрос успешно принят");
        }

        [HttpPost("reject/{eventId}/{librarianId}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> RejectRequest(int eventId, int librarianId)
        {
            await _bookEventService.RejectRequest(eventId, librarianId);
            return Ok("Запрос отклонен");
        }

        [HttpPost("save")]
        public async Task<ActionResult> SaveBook(int accountId, int bookId)
        {
            await _bookEventService.SaveBook(accountId, bookId);
            return Ok();
        }

        [HttpPost("take")]
        public async Task<ActionResult> TakeBook(int accountId, int bookId)
        {
            await _bookEventService.TakeBook(accountId, bookId);
            return Ok();
        }

        [HttpPost("return/{eventId}")]
        public async Task<ActionResult> ReturnBook(int eventId)
        {
            await _bookEventService.ReturnBook(eventId);
            return Ok();
        }

        [HttpDelete("remove-saved")]
        public async Task<ActionResult> RemoveSavedBook(int accountId, int bookId)
        {
            await _bookEventService.RemoveSavedBook(accountId, bookId);
            return Ok();
        }

        [HttpDelete("cancel-request")]
        public async Task<ActionResult> CancelRequest(int accountId, int bookId)
        {
            await _bookEventService.CancelRequest(accountId, bookId);
            return Ok();
        }

        [HttpGet]
        [Route("taked")]
        public async Task<ActionResult<List<BookEventListDTO>>> GetTakedBooks()
        {
            var books = await _bookEventService.GetAllTakedBooks();
            return Ok(books);
        }

        [HttpGet]
        [Route("request/{eventId}/detail")]
        public async Task<ActionResult<BookRequestDetailDTO>> GetRequestDetail(int eventId)
        {
            try
            {
                var request = await _bookEventService.GetRequestDetail(eventId);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 