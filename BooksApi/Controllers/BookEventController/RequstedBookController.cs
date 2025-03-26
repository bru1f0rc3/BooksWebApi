using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using BooksApi.Service.BookEventService;
using Microsoft.AspNetCore.Mvc;
using Supabase;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/requested")]
    [ApiController]
    public class RequstedBookController : ControllerBase
    {
        private readonly RequstedTakedBookService _request;
        private readonly Client _supabaseClient;

        public RequstedBookController(RequstedTakedBookService request, Client supabaseClient)
        {
            _request = request;
            _supabaseClient = supabaseClient;
        }

        [HttpPost]
        public async Task<IActionResult> RequestedBookTask([FromBody] Requested_BooksDTO req)
        {
            var query = new Requested_Books { BookId = req.BookId, AccountId = req.AccountId };
            await _request.RequestBook(query);
            return Ok(new { Message = "Книга успешно запрошена" });
        }

       [HttpGet("list/requested")]
        public async Task<IActionResult> GetRequestedBooks()
        {
            var requests = await _supabaseClient
                .From<Requested_Books>()
                .Select("*")
                .Get();
            
            var response = requests.Models.Select(r => new Requested_BooksDTO
            {
                Id = r.Id,
                BookId = r.BookId,
                AccountId = r.AccountId,
                CreatedAt = r.CreatedAt
            }).ToList();
            
            return Ok(response);
        }

        [HttpPost("approve/{requestId}")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var request = await _supabaseClient
                .From<Requested_Books>()
                .Where(x => x.Id == requestId)
                .Get();

            if (request.Models.Count == 0)
                return NotFound(new { Message = "Запрос не найден" });

            var requestedBook = request.Models[0];

            await _request.TakedBook(new Taken_Books 
            { 
                BookId = requestedBook.BookId, 
                AccountId = requestedBook.AccountId 
            });

            // Удаляем запрос
            await _supabaseClient
                .From<Requested_Books>()
                .Where(x => x.Id == requestId)
                .Delete();

            return Ok(new { Message = "Запрос одобрен" });
        }

        [HttpPost("reject/{requestId}")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var request = await _supabaseClient
                .From<Requested_Books>()
                .Where(x => x.Id == requestId)
                .Get();

            if (request.Models.Count == 0)
                return NotFound(new { Message = "Запрос не найден" });

            await _supabaseClient
                .From<Requested_Books>()
                .Where(x => x.Id == requestId)
                .Delete();

            return Ok(new { Message = "Запрос отклонен" });
        }
    }
}
