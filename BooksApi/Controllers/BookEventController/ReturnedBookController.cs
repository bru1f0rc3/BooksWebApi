using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/returned")]
    [ApiController]
    public class ReturnedBookController : ControllerBase
    {
        private readonly ReturnedSavedBookService _request;

        public ReturnedBookController(ReturnedSavedBookService request)
        {
            _request = request;
        }

        [HttpPost]
        public async Task<IActionResult> ReturnedBookTask([FromBody] Returned_BooksDTO req)
        {
            var query = new Returned_Books { BookId = req.BookId, AccountId = req.AccountId };
            await _request.ReturnedBook(query);
            return Ok(new { Message = "Book returned successfully" });
        }
    }
}
