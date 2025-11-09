using Microsoft.AspNetCore.Http;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с файлами (загрузка, удаление изображений)
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Сохранить изображение на диск
        /// </summary>
        /// <param name="image">Файл изображения</param>
        /// <returns>Имя сохраненного файла</returns>
        Task<string> SaveImage(IFormFile image);

        /// <summary>
        /// Удалить изображение с диска
        /// </summary>
        /// <param name="fileName">Имя файла для удаления</param>
        void DeleteImage(string fileName);
    }
}
