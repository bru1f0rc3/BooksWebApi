using BooksApi.Service.BookService;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.Books
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetBookIdDetailsController : ControllerBase
    {
        private readonly GetBookIdDetailsService _getBookIdDetailsService;

        public GetBookIdDetailsController(GetBookIdDetailsService getBookIdDetailsService)
        {
            _getBookIdDetailsService = getBookIdDetailsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetDetailsBookTask(int id)
        {
            try
            {
                var bookDetails = await _getBookIdDetailsService.GetBookIdDetailsTask(id);
                if (bookDetails == null || !bookDetails.Any())
                {
                    return NotFound(new { message = "Книга не найдена" });
                }
                return Ok(bookDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при загрузке книги", error = ex.Message });
            }
        }
    }
}
