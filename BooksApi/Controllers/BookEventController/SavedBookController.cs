using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BooksApi.Service.BookEventService;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/save")]
    [ApiController]
    [Authorize]
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
            try
            {
                var query = new Saved_Books { BookId = req.BookId, AccountId = req.AccountId };
                await _request.SavedBook(query);
                return Ok(new { Message = "Книга успешно сохранена" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ошибка при сохранении книги", Error = ex.Message });
            }
        }

        [HttpGet("user/{accountId}")]
        public async Task<IActionResult> GetUserSavedBooks(int accountId)
        {
            try
            {
                var savedBooks = await _request.GetUserSavedBooks(accountId);
                return Ok(new { Books = savedBooks });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ошибка при получении сохраненных книг", Error = ex.Message });
            }
        }
    }
}
