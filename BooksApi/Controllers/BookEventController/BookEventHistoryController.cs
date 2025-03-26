using Microsoft.AspNetCore.Mvc;
using BooksApi.Service.BookEventService;
using BooksApi.DTO.EventBook;
using BooksApi.Models.Dashboard;
using Supabase;
using BooksApi.Models.Book;
using BooksApi.DTO.Books;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/events")]
    [ApiController]
    public class BookEventHistoryController : ControllerBase
    {
        private readonly BookEventService _eventService;
        private readonly Client _supabaseClient;

        public BookEventHistoryController(BookEventService eventService, Client supabaseClient)
        {
            _eventService = eventService;
            _supabaseClient = supabaseClient;
        }

        [HttpGet("user/{accountId}")]
        public async Task<IActionResult> GetUserBookEvents(int accountId)
        {
            try
            {
                var events = await _eventService.GetUserBookEvents(accountId);
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
            var events = await _eventService.GetAllBookEvents();

            var history = new List<BookEventHistoryDTO>();

            foreach (var evt in events)
            {
                // Получаем информацию о книге
                var book = await _supabaseClient
                    .From<Bookss>()
                    .Match(new Dictionary<string, string> { { "id", evt.BookId.ToString() } })
                    .Get();

                // Получаем информацию о пользователе
                var user = await _supabaseClient
                    .From<Account>()
                    .Match(new Dictionary<string, string> { { "id", evt.AccountId.ToString() } })
                    .Get();

                if (book.Models.Count > 0 && user.Models.Count > 0)
                {
                    // Получаем информацию об авторе
                    var author = await _supabaseClient
                        .From<Authors>()
                        .Match(new Dictionary<string, string> { { "id", book.Models[0].Author.ToString() } })
                        .Get();

                    history.Add(new BookEventHistoryDTO
                    {
                        Id = evt.Id,
                        BookTitle = book.Models[0].Title,
                        BookAuthor = author.Models.Count > 0 ? author.Models[0].FullName : "Неизвестный автор",
                        UserLogin = user.Models[0].Login,
                        UserFullName = user.Models[0].FullName,
                        EventType = evt.EventType,
                        CreatedAt = evt.CreatedAt
                    });
                }
            }

            return Ok(new { events = history });
        }
    }
} 