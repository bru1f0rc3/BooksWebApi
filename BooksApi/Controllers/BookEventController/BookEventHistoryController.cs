using Microsoft.AspNetCore.Mvc;
using BooksApi.Service.BookEventService;
using BooksApi.DTO.EventBook;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/events")]
    [ApiController]
    public class BookEventHistoryController : ControllerBase
    {
        private readonly BookEventService _bookEventService;

        public BookEventHistoryController(BookEventService bookEventService)
        {
            _bookEventService = bookEventService;
        }

        [HttpGet("user/{accountId}")]
        public async Task<IActionResult> GetUserBookEvents(int accountId)
        {
            try
            {
                var events = await _bookEventService.GetUserBookEvents(accountId);
                return Ok(new { events });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении истории событий", error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookEvents()
        {
            try
            {
                var events = await _bookEventService.GetAllBookEvents();
                return Ok(new { events });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении истории событий", error = ex.Message });
            }
        }
    }
} 