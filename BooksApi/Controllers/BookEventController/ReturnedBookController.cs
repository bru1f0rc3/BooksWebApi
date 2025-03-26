using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BooksApi.Service.BookEventService;
using Microsoft.Extensions.Logging;

namespace BooksApi.Controllers.BookEventController
{
    [Route("api/book/returned")]
    [ApiController]
    [Authorize]
    public class ReturnedBookController : ControllerBase
    {
        private readonly ReturnedSavedBookService _request;
        private readonly ILogger<ReturnedBookController> _logger;

        public ReturnedBookController(ReturnedSavedBookService request, ILogger<ReturnedBookController> logger)
        {
            _request = request;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReturnedBookTask([FromBody] Returned_BooksDTO req)
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

                var query = new Returned_Books { BookId = req.BookId, AccountId = req.AccountId };
                await _request.ReturnedBook(query);
                return Ok(new { Message = "Книга успешно возвращена" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при возврате книги");
                return StatusCode(500, new { Message = "Ошибка при возврате книги", Error = ex.Message });
            }
        }

        [HttpGet("user/{accountId}")]
        public async Task<IActionResult> GetUserReturnedBooks(int accountId)
        {
            try
            {
                if (accountId <= 0)
                {
                    return BadRequest(new { Message = "Некорректный ID пользователя" });
                }

                var returnedBooks = await _request.GetUserReturnedBooks(accountId);
                if (returnedBooks == null || !returnedBooks.Any())
                {
                    return NotFound(new { Message = "У пользователя нет возвращенных книг" });
                }

                return Ok(new { Books = returnedBooks });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении возвращенных книг");
                return StatusCode(500, new { Message = "Ошибка при получении возвращенных книг", Error = ex.Message });
            }
        }
    }
}
