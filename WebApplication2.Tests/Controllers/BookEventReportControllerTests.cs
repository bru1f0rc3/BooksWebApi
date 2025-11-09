using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers.BookEvent;
using WebApplication2.DTO.BookEvent;
using WebApplication2.Interfaces;

namespace WebApplication2.Tests
{
    public class BookEventReportControllerTests
    {
        private readonly Mock<IBookEventReportService> _mockReportService;
        private readonly BookEventReportController _controller;

        public BookEventReportControllerTests()
        {
            _mockReportService = new Mock<IBookEventReportService>();
            _controller = new BookEventReportController(_mockReportService.Object);
        }

        [Fact]
        public async Task GenerateReport_WithValidFilter_ReturnsFileResult()
        {
            // Arrange
            var filter = new BookEventFilterDTO
            {
                EventTypeId = 1,
                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now
            };
            var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF header bytes
            _mockReportService.Setup(s => s.GeneratePdfReportAsync(filter)).ReturnsAsync(pdfBytes);

            // Act
            var result = await _controller.GenerateReport(filter);

            // Assert
            var fileResult = result.Should().BeOfType<FileContentResult>().Subject;
            fileResult.ContentType.Should().Be("application/pdf");
            fileResult.FileDownloadName.Should().Be("BookEventsReport.pdf");
            fileResult.FileContents.Should().Equal(pdfBytes);
        }

        [Fact]
        public async Task GenerateReport_WithNullFilter_ReturnsFileResult()
        {
            // Arrange
            var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
            _mockReportService.Setup(s => s.GeneratePdfReportAsync(It.IsAny<BookEventFilterDTO>()))
                .ReturnsAsync(pdfBytes);

            // Act
            var result = await _controller.GenerateReport(new BookEventFilterDTO());

            // Assert
            var fileResult = result.Should().BeOfType<FileContentResult>().Subject;
            fileResult.ContentType.Should().Be("application/pdf");
        }

        [Fact]
        public async Task GenerateReport_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var filter = new BookEventFilterDTO();
            _mockReportService.Setup(s => s.GeneratePdfReportAsync(filter))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await _controller.GenerateReport(filter);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Error generating report: Database connection error");
        }

        [Fact]
        public async Task GenerateReport_WithDateRangeFilter_CallsServiceWithCorrectParameters()
        {
            // Arrange
            var filter = new BookEventFilterDTO
            {
                EventTypeId = 2,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 12, 31)
            };
            var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
            _mockReportService.Setup(s => s.GeneratePdfReportAsync(It.IsAny<BookEventFilterDTO>()))
                .ReturnsAsync(pdfBytes);

            // Act
            var result = await _controller.GenerateReport(filter);

            // Assert
            _mockReportService.Verify(s => s.GeneratePdfReportAsync(It.Is<BookEventFilterDTO>(
                f => f.EventTypeId == 2 &&
                     f.StartDate == new DateTime(2024, 1, 1) &&
                     f.EndDate == new DateTime(2024, 12, 31)
            )), Times.Once);
        }

        [Fact]
        public async Task GenerateReport_WithEmptyResult_ReturnsEmptyFile()
        {
            // Arrange
            var filter = new BookEventFilterDTO();
            var emptyPdfBytes = new byte[0];
            _mockReportService.Setup(s => s.GeneratePdfReportAsync(filter))
                .ReturnsAsync(emptyPdfBytes);

            // Act
            var result = await _controller.GenerateReport(filter);

            // Assert
            var fileResult = result.Should().BeOfType<FileContentResult>().Subject;
            fileResult.FileContents.Should().BeEmpty();
        }
    }
}
