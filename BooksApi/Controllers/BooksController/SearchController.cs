using BooksApi.Service.BookService;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.BooksController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookTask(string searchQuery)
        {
            try
            {
                var books = await _searchService.SearchBooksTask(searchQuery);
                return Ok(new { books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при загрузке книг", error = ex.Message });
            }
        }

    }
}
