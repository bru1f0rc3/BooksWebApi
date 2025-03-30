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

        [Route("return")]
        [HttpPost]
        public async Task<ActionResult> ReturnBook(int accountId, int bookId)
        {
            await _bookEventService.ReturnBook(accountId, bookId);
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
    }
} 