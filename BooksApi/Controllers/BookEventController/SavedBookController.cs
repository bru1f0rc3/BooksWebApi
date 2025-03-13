using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/save")]
    [ApiController]
    public class SavedBookController : ControllerBase
    {
        private readonly ReturnedSavedBookService _request;

        public SavedBookController(ReturnedSavedBookService request)
        {
            _request = request;
        }

        [HttpPost]
        public async Task<IActionResult> SavedBookTask([FromBody] Saved_BooksDTO req)
        {
            var query = new Saved_Books { BookId = req.BookId, AccountId = req.AccountId };
            await _request.SavedBook(query);
            return Ok(new { Message = "Book saved successfully" });
        }
    }
}
