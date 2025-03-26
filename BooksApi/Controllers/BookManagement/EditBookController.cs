using Microsoft.AspNetCore.Mvc;
using BooksApi.Service.BookManagement;
using BooksApi.DTO.Books;

namespace BooksApi.Controllers.BookManagement
{
    [Route("api/book/management")]
    [ApiController]
    public class EditBookController : ControllerBase
    {
        private readonly EditBookService _editBookService;

        public EditBookController(EditBookService editBookService)
        {
            _editBookService = editBookService;
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditBook([FromBody] BooksDTO book)
        {
            try
            {
                var result = await _editBookService.EditBookTask(book);
                return Ok(new { message = "Книга успешно отредактирована", book = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при редактировании книги", error = ex.Message });
            }
        }
    }
} 