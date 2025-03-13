using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using BooksApi.Service.DashboardService;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/requested")]
    [ApiController]
    public class RequstedBookController : ControllerBase
    {
        private readonly RequstedTakedBookService _request;

        public RequstedBookController(RequstedTakedBookService request)
        {
            _request = request;
        }

        [HttpPost]
        public async Task<IActionResult> RequestedBookTask([FromBody] Requested_BooksDTO req)
        {
            var query = new Requested_Books { BookId = req.BookId, AccountId = req.AccountId };
            await _request.RequestBook(query);
            return Ok(new { Message = "Book requested successfully" });
        }
    }
}
