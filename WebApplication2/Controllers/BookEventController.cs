using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.BookEvent;
using WebApplication2.Services.BookEvent;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/book-event")]
    public class BookEventController : ControllerBase
    {
        private readonly BookEventService _bookEventService;

        public BookEventController(BookEventService bookEventService)
        {
            _bookEventService = bookEventService;
        }

        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<List<BookEventDTO>>> GetBookEvents()
        {
            var events = await _bookEventService.GetBookEvents();
            return Ok(events);
        }

        [Route("pending-requests")]
        [HttpGet]
        public async Task<ActionResult<List<BookEventDTO>>> GetPendingRequests()
        {
            var requests = await _bookEventService.GetPendingRequests();
            return Ok(requests);
        }

        [Route("user/{accountId}/history")]
        [HttpGet]
        public async Task<ActionResult<List<UserBookHistoryDTO>>> GetUserBookHistory(int accountId)
        {
            var history = await _bookEventService.GetUserBookHistory(accountId);
            return Ok(history);
        }

        [Route("user/{accountId}/current-books")]
        [HttpGet]
        public async Task<ActionResult<List<UserBookHistoryDTO>>> GetUserCurrentBooks(int accountId)
        {
            var currentBooks = await _bookEventService.GetUserCurrentBooks(accountId);
            return Ok(currentBooks);
        }

        [Route("request")]
        [HttpPost]
        public async Task<ActionResult> RequestBook(int accountId, int bookId)
        {
            await _bookEventService.RequestBook(accountId, bookId);
            return Ok();
        }

        [Route("accept/{eventId}/{librarianId}")]
        [HttpPost]
        public async Task<ActionResult> AcceptRequest(int eventId, int librarianId)
        {
            var result = await _bookEventService.AcceptRequest(eventId, librarianId);
            
            if (!result)
                return BadRequest("Запрос не найден или уже обработан");

            return Ok("Запрос успешно принят");
        }

        [Route("reject/{eventId}")]
        [HttpPost]
        public async Task<ActionResult> RejectRequest(int eventId, int librarianId)
        {
            await _bookEventService.RejectRequest(eventId, librarianId);
            return Ok();
        }

        [Route("save")]
        [HttpPost]
        public async Task<ActionResult> SaveBook(int accountId, int bookId)
        {
            await _bookEventService.SaveBook(accountId, bookId);
            return Ok();
        }

        [Route("take")]
        [HttpPost]
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

        [Route("remove-saved")]
        [HttpDelete]
        public async Task<ActionResult> RemoveSavedBook(int accountId, int bookId)
        {
            await _bookEventService.RemoveSavedBook(accountId, bookId);
            return Ok();
        }

        [Route("cancel-request")]
        [HttpDelete]
        public async Task<ActionResult> CancelRequest(int accountId, int bookId)
        {
            await _bookEventService.CancelRequest(accountId, bookId);
            return Ok();
        }

        [HttpGet("active-requests")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<List<BookEventListDTO>>> GetActiveRequests()
        {
            var requests = await _bookEventService.GetActiveRequests();
            return Ok(requests);
        }

        [HttpGet("taked-books")]
        public async Task<ActionResult<List<TakedBookDTO>>> GetTakedBooks()
        {
            var takedBooks = await _bookEventService.GetTakedBooks();
            return Ok(takedBooks);
        }

        [HttpGet("user/{accountId}/requested")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserRequestedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 1); // 1 = Requested
            return Ok(books);
        }

        [HttpGet("user/{accountId}/returned")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserReturnedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 2); // 2 = Returned
            return Ok(books);
        }

        [HttpGet("user/{accountId}/taked")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserTakedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 3); // 3 = Taked
            return Ok(books);
        }

        [HttpGet("user/{accountId}/saved")]
        public async Task<ActionResult<List<UserBookEventDTO>>> GetUserSavedBooks(int accountId)
        {
            var books = await _bookEventService.GetUserBooksByEventType(accountId, 4); // 4 = Saved
            return Ok(books);
        }
    }
} 