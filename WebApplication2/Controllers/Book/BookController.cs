using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Book;
using WebApplication2.Interfaces;
using Microsoft.AspNetCore.Http;

namespace WebApplication2.Controllers.Book
{
    /// <summary>
    /// Контроллер для управления книгами
    /// </summary>
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        /// <summary>
        /// Конструктор контроллера книг
        /// </summary>
        /// <param name="bookService">Сервис для работы с книгами</param>
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Получить список всех книг
        /// </summary>
        /// <returns>Список книг с основной информацией</returns>
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<List<BookListDTO>>> GetBooks()
        {
            var books = await _bookService.BookListedGet();
            return Ok(books);
        }

        /// <summary>
        /// Добавить новую книгу в библиотеку
        /// </summary>
        /// <param name="book">Данные новой книги</param>
        /// <param name="coverImage">Файл изображения обложки (опционально)</param>
        /// <returns>Результат операции добавления</returns>
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

        /// <summary>
        /// Редактировать существующую книгу
        /// </summary>
        /// <param name="book">Обновленные данные книги</param>
        /// <param name="coverImage">Новый файл изображения обложки (опционально)</param>
        /// <returns>Результат операции редактирования</returns>
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

        /// <summary>
        /// Удалить книгу по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор книги для удаления</param>
        /// <returns>Результат операции удаления</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveBook(int id)
        {
            await _bookService.RemoveBook(id);
            return Ok();
        }

        /// <summary>
        /// Поиск книг по названию, автору или категории
        /// </summary>
        /// <param name="searchTerm">Поисковый запрос (название книги или имя автора)</param>
        /// <param name="categoryId">Идентификатор категории для фильтрации (опционально)</param>
        /// <returns>Список найденных книг</returns>
        [Route("search")]
        [HttpGet]
        public async Task<ActionResult<List<BookListDTO>>> SearchBooks([FromQuery] string? searchTerm, [FromQuery] int? categoryId)
        {
            var books = await _bookService.SearchBooks(searchTerm, categoryId);
            return Ok(books);
        }

        /// <summary>
        /// Получить детальную информацию о книге
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <returns>Детальная информация о книге</returns>
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