using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.BookEvent;
using WebApplication2.Interfaces;

namespace WebApplication2.Controllers.BookEvent
{
    /// <summary>
    /// Контроллер для генерации отчетов по событиям книг
    /// </summary>
    [ApiController]
    [Route("api/book-event-report")]
    public class BookEventReportController : ControllerBase
    {
        private readonly IBookEventReportService _reportService;

        /// <summary>
        /// Конструктор контроллера отчетов
        /// </summary>
        public BookEventReportController(IBookEventReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("generate")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> GenerateReport([FromQuery] BookEventFilterDTO filter)
        {
            try
            {
                var pdfBytes = await _reportService.GeneratePdfReportAsync(filter);
                return File(pdfBytes, "application/pdf", "BookEventsReport.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating report: {ex.Message}");
            }
        }
    }
} 