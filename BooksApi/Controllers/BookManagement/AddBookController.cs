using Microsoft.AspNetCore.Mvc;
using BooksApi.Service.BookManagement;
using BooksApi.DTO.Books;

namespace BooksApi.Controllers.BookManagement
{
    [Route("api/book/management")]
    [ApiController]
    public class AddBookController : ControllerBase
    {
        private readonly AddBookService _addBookService;

        public AddBookController(AddBookService addBookService)
        {
            _addBookService = addBookService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBook([FromBody] BooksDTO book)
        {
            try
            {
                var result = await _addBookService.AddBookTask(book);
                return Ok(new { message = "Книга успешно добавлена", book = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при добавлении книги", error = ex.Message });
            }
        }
    }
} 