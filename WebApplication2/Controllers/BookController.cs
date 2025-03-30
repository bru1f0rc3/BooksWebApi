using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Book;
using WebApplication2.Services.Book;

namespace WebApplication2.Controllers
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
        public async Task<ActionResult<List<BookListDTO>>> GetBooks()
        {
            var books = await _bookService.BookListedGet();
            return Ok(books);
        }
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddBook(AddBook book)
        {
            await _bookService.AddBook(book);
            return Ok();
        }
        [Route("edit")]
        [HttpPut]
        public async Task<ActionResult> EditBook(EditBook book)
        {
            await _bookService.EditBook(book);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveBook(int id)
        {
            await _bookService.RemoveBook(id);
            return Ok();
        }
        [Route("search")]
        [HttpGet]
        public async Task<ActionResult<List<BookListDTO>>> SearchBooks([FromQuery] string? searchTerm, [FromQuery] int? categoryId)
        {
            var books = await _bookService.SearchBooks(searchTerm, categoryId);
            return Ok(books);
        }
    }
} 