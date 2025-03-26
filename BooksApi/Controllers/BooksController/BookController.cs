using BooksApi.Service.BookService;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.Books
{
    [Route("api/bookall")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ListedBookService _bookService;

        public BookController(ListedBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookTask()
        {
            try
            {
                var books = await _bookService.ListedBookTask();
                return Ok(new { books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при загрузке книг", error = ex.Message });
            }
        }
    }
}