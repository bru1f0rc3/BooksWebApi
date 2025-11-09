using WebApplication2.DTO.BookEvent;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для генерации отчетов по событиям книг
    /// </summary>
    public interface IBookEventReportService
    {
        /// <summary>
        /// Сгенерировать PDF отчет по событиям книг
        /// </summary>
        /// <param name="filter">Фильтр для отчета</param>
        /// <returns>PDF файл отчета в виде массива байтов</returns>
        Task<byte[]> GeneratePdfReportAsync(BookEventFilterDTO filter);
    }
}
