using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Book;
using WebApplication2.Services.Book;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> AddBook([FromForm] AddBook book, IFormFile? coverImage)
        {
            try
            {
                await _bookService.AddBook(book, coverImage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("edit")]
        [HttpPut]
        public async Task<IActionResult> EditBook([FromForm] EditBook book, IFormFile? coverImage)
        {
            try
            {
                await _bookService.EditBook(book, coverImage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        [HttpGet]
        [Route("{id}/detail")]
        public async Task<ActionResult<BookDetailDTO>> GetBookDetail(int id)
        {
            try
            {
                var book = await _bookService.GetBookDetail(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 