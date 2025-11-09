using Microsoft.AspNetCore.Http;
using WebApplication2.DTO.Book;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с книгами
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Получить полный список всех книг с информацией об авторах, категориях и филиалах
        /// </summary>
        /// <returns>Список всех книг в библиотеке</returns>
        Task<List<BookListDTO>> BookListedGet();

        /// <summary>
        /// Добавить новую книгу в базу данных
        /// </summary>
        /// <param name="book">Данные новой книги</param>
        /// <param name="coverImage">Файл изображения обложки книги (опционально)</param>
        Task AddBook(AddBook book, IFormFile? coverImage);

        /// <summary>
        /// Редактировать существующую книгу в базе данных
        /// </summary>
        /// <param name="book">Обновленные данные книги</param>
        /// <param name="coverImage">Новый файл изображения обложки (опционально)</param>
        Task EditBook(EditBook book, IFormFile? coverImage);

        /// <summary>
        /// Удалить книгу из базы данных по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор книги для удаления</param>
        Task RemoveBook(int id);

        /// <summary>
        /// Поиск книг по названию, автору или категории
        /// </summary>
        /// <param name="searchTerm">Поисковый запрос для поиска по названию или автору</param>
        /// <param name="categoryId">Идентификатор категории для фильтрации (опционально)</param>
        /// <returns>Список книг, соответствующих критериям поиска</returns>
        Task<List<BookListDTO>> SearchBooks(string? searchTerm, int? categoryId = null);

        /// <summary>
        /// Получить детальную информацию о конкретной книге
        /// </summary>
        /// <param name="bookId">Идентификатор книги</param>
        /// <returns>Детальная информация о книге</returns>
        /// <exception cref="Exception">Выбрасывается, если книга не найдена</exception>
        Task<BookDetailDTO> GetBookDetail(int bookId);
    }
}
