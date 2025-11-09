using WebApplication2.DTO.Branch;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с филиалами библиотеки
    /// </summary>
    public interface IBranchService
    {
        /// <summary>
        /// Получить список всех филиалов
        /// </summary>
        /// <returns>Список филиалов</returns>
        Task<List<BranchDTO>> GetAllBranches();

        /// <summary>
        /// Получить филиал по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор филиала</param>
        /// <returns>Данные филиала или null, если не найден</returns>
        Task<BranchDTO?> GetBranchById(int id);

        /// <summary>
        /// Создать новый филиал
        /// </summary>
        /// <param name="branch">Данные для создания филиала</param>
        /// <returns>Созданный филиал</returns>
        Task<BranchDTO> CreateBranch(CreateBranchDTO branch);

        /// <summary>
        /// Обновить данные филиала
        /// </summary>
        /// <param name="id">Идентификатор филиала</param>
        /// <param name="branch">Обновленные данные филиала</param>
        /// <returns>Обновленный филиал или null, если не найден</returns>
        Task<BranchDTO?> UpdateBranch(int id, UpdateBranchDTO branch);

        /// <summary>
        /// Удалить филиал
        /// </summary>
        /// <param name="id">Идентификатор филиала</param>
        /// <returns>True, если филиал был удален, иначе false</returns>
        Task<bool> DeleteBranch(int id);
    }
}
