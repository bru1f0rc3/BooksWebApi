using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Book;
using WebApplication2.Services.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
namespace WebApplication2.Controllers.Book
{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }
        [Route("list")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<BookListDTO>>> GetBooks()
        {
            var books = await _bookService.BookListedGet();
            return Ok(books);
        }
        [Route("add")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBook([FromForm] AddBook book, IFormFile? coverImage)
        {
            await _bookService.AddBook(book, coverImage);
            return Ok();
        }
        [Route("edit")]
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBook([FromForm] EditBook book, IFormFile? coverImage)
        {
            await _bookService.EditBook(book, coverImage);
            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveBook(int id)
        {
            await _bookService.RemoveBook(id);
            return Ok();
        }
        [Route("search")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<BookListDTO>>> SearchBooks([FromQuery] string? searchTerm, [FromQuery] int? categoryId)
        {
            var books = await _bookService.SearchBooks(searchTerm, categoryId);
            return Ok(books);
        }
        [HttpGet]
        [Route("{id}/detail")]
        [AllowAnonymous]
        public async Task<ActionResult<BookDetailDTO>> GetBookDetail(int id)
        {
            var book = await _bookService.GetBookDetail(id);
            return Ok(book);
        }
    }
}
