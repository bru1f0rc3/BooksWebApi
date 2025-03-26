using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BooksApi.Service.BookEventService;
using Microsoft.Extensions.Logging;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/save")]
    [ApiController]
    [Authorize]
    public class SavedBookController : ControllerBase
    {
        private readonly ReturnedSavedBookService _request;
        private readonly ILogger<SavedBookController> _logger;

        public SavedBookController(ReturnedSavedBookService request, ILogger<SavedBookController> logger)
        {
            _request = request;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SavedBookTask([FromBody] Saved_BooksDTO req)
        {
            try
            {
                if (req == null)
                {
                    return BadRequest(new { Message = "Данные запроса не могут быть пустыми" });
                }

                if (req.BookId <= 0 || req.AccountId <= 0)
                {
                    return BadRequest(new { Message = "Некорректные ID книги или пользователя" });
                }

                var query = new Saved_Books { BookId = req.BookId, AccountId = req.AccountId };
                await _request.SavedBook(query);
                return Ok(new { Message = "Книга успешно сохранена" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении книги");
                return StatusCode(500, new { Message = "Ошибка при сохранении книги", Error = ex.Message });
            }
        }

        [HttpGet("user/{accountId}")]
        public async Task<IActionResult> GetUserSavedBooks(int accountId)
        {
            try
            {
                if (accountId <= 0)
                {
                    return BadRequest(new { Message = "Некорректный ID пользователя" });
                }

                var savedBooks = await _request.GetUserSavedBooks(accountId);
                if (savedBooks == null || !savedBooks.Any())
                {
                    return NotFound(new { Message = "У пользователя нет сохраненных книг" });
                }

                return Ok(new { Books = savedBooks });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении сохраненных книг");
                return StatusCode(500, new { Message = "Ошибка при получении сохраненных книг", Error = ex.Message });
            }
        }
    }
}
