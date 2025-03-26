using Microsoft.AspNetCore.Mvc;
using BooksApi.Service.BookManagement;

namespace BooksApi.Controllers.BookManagement
{
    [Route("api/book/management")]
    [ApiController]
    public class DeleteBookController : ControllerBase
    {
        private readonly DeleteBookService _deleteBookService;

        public DeleteBookController(DeleteBookService deleteBookService)
        {
            _deleteBookService = deleteBookService;
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _deleteBookService.DeleteBookTask(id);
                return Ok(new { message = "Книга успешно удалена" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при удалении книги", error = ex.Message });
            }
        }
    }
} 