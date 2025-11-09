using WebApplication2.DTO.Author;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с авторами книг
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Получить список всех авторов
        /// </summary>
        /// <returns>Список авторов</returns>
        Task<List<AuthorDTO>> GetAllAuthors();

        /// <summary>
        /// Получить автора по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор автора</param>
        /// <returns>Данные автора или null, если не найден</returns>
        Task<AuthorDTO?> GetAuthorById(int id);

        /// <summary>
        /// Создать нового автора
        /// </summary>
        /// <param name="author">Данные для создания автора</param>
        /// <returns>Созданный автор</returns>
        Task<AuthorDTO> CreateAuthor(CreateAuthorDTO author);

        /// <summary>
        /// Обновить данные автора
        /// </summary>
        /// <param name="id">Идентификатор автора</param>
        /// <param name="author">Обновленные данные автора</param>
        /// <returns>Обновленный автор или null, если не найден</returns>
        Task<AuthorDTO?> UpdateAuthor(int id, UpdateAuthorDTO author);

        /// <summary>
        /// Удалить автора
        /// </summary>
        /// <param name="id">Идентификатор автора</param>
        /// <returns>True, если автор был удален, иначе false</returns>
        Task<bool> DeleteAuthor(int id);
    }
}
