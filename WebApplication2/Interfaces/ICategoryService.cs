using WebApplication2.DTO.Category;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с категориями книг
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Получить список всех категорий
        /// </summary>
        /// <returns>Список категорий</returns>
        Task<List<CategoryDTO>> GetAllCategories();

        /// <summary>
        /// Получить категорию по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns>Данные категории или null, если не найдена</returns>
        Task<CategoryDTO?> GetCategoryById(int id);

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        /// <param name="category">Данные для создания категории</param>
        /// <returns>Созданная категория</returns>
        Task<CategoryDTO> CreateCategory(CreateCategoryDTO category);

        /// <summary>
        /// Обновить данные категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="category">Обновленные данные категории</param>
        /// <returns>Обновленная категория или null, если не найдена</returns>
        Task<CategoryDTO?> UpdateCategory(int id, UpdateCategoryDTO category);

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns>True, если категория была удалена, иначе false</returns>
        Task<bool> DeleteCategory(int id);
    }
}
