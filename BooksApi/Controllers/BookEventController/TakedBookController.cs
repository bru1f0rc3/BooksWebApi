using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using BooksApi.Service.BookEventService;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/taken")]
    [ApiController]
    public class TakedBookController : ControllerBase
    {
        private readonly RequstedTakedBookService _request;

        public TakedBookController(RequstedTakedBookService request)
        {
            _request = request;
        }

        [HttpPost]
        public async Task<IActionResult> TakenBookTask([FromBody] Taken_BooksDTO req)
        {
            var query = new Taken_Books { BookId = req.BookId, AccountId = req.AccountId };
            await _request.TakedBook(query);
            return Ok(new { Message = "Book taken successfully" });
        }
    }
}
