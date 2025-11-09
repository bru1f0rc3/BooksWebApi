using WebApplication2.DTO.BookEvent;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с событиями книг (выдача, возврат, бронирование)
    /// </summary>
    public interface IBookEventService
    {
        /// <summary>
        /// Получить список всех событий книг
        /// </summary>
        Task<List<BookEventListDTO>> GetBookEvents();

        /// <summary>
        /// Получить список активных запросов на книги
        /// </summary>
        Task<List<BookEventListDTO>> GetActiveRequests();

        /// <summary>
        /// Получить историю книг пользователя
        /// </summary>
        Task<List<UserBookHistoryDTO>> GetUserBookHistory(int accountId);

        /// <summary>
        /// Получить книги пользователя по типу события
        /// </summary>
        Task<List<UserBookEventDTO>> GetUserBooksByEventType(int accountId, int eventTypeId);

        /// <summary>
        /// Запросить книгу
        /// </summary>
        Task RequestBook(int accountId, int bookId);

        /// <summary>
        /// Принять запрос на книгу
        /// </summary>
        Task<bool> AcceptRequest(int eventId, int librarianId);

        /// <summary>
        /// Отклонить запрос на книгу
        /// </summary>
        Task RejectRequest(int eventId, int librarianId);

        /// <summary>
        /// Сохранить книгу в избранное
        /// </summary>
        Task SaveBook(int accountId, int bookId);

        /// <summary>
        /// Взять книгу
        /// </summary>
        Task TakeBook(int accountId, int bookId);

        /// <summary>
        /// Вернуть книгу
        /// </summary>
        Task ReturnBook(int eventId);

        /// <summary>
        /// Удалить книгу из сохраненных
        /// </summary>
        Task RemoveSavedBook(int accountId, int bookId);

        /// <summary>
        /// Отменить запрос на книгу
        /// </summary>
        Task CancelRequest(int accountId, int bookId);

        /// <summary>
        /// Получить список всех взятых книг
        /// </summary>
        Task<List<BookEventListDTO>> GetAllTakedBooks();

        /// <summary>
        /// Получить детальную информацию о запросе
        /// </summary>
        Task<BookRequestDetailDTO> GetRequestDetail(int eventId);
    }
}
