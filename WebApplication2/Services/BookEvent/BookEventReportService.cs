using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WebApplication2.Connection;
using WebApplication2.DTO.BookEvent;
using System.Text;
using Dapper;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.BookEvent
{
    /// <summary>
    /// Сервис для генерации отчетов по событиям книг
    /// </summary>
    public class BookEventReportService : IBookEventReportService
    {
        private readonly string _connectionString;

        public BookEventReportService()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public async Task<byte[]> GeneratePdfReportAsync(BookEventFilterDTO filter)
        {
            try
            {
                filter ??= new BookEventFilterDTO();
                
                var events = await GetFilteredEventsAsync(filter);
                if (!events.Any())
                {
                    throw new Exception("No events found for the specified criteria");
                }

                using (var document = new PdfDocument())
                {
                    document.Info.Title = "Book Events Report";
                    var page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;
                    page.Orientation = PdfSharp.PageOrientation.Landscape;
                    
                    var gfx = XGraphics.FromPdfPage(page);
                    var font = new XFont("Arial", 10);
                    var boldFont = new XFont("Arial", 10);
                    var titleFont = new XFont("Arial", 16);
                    double margin = 50;
                    double usableWidth = page.Width - 2 * margin;
                    
                    var columnWidths = new[]
                    {
                        usableWidth * 0.12,
                        usableWidth * 0.23, 
                        usableWidth * 0.25, 
                        usableWidth * 0.20,
                        usableWidth * 0.20 
                    };

                    var columnX = new double[5];
                    columnX[0] = margin;
                    for (int i = 1; i < 5; i++)
                    {
                        columnX[i] = columnX[i - 1] + columnWidths[i - 1];
                    }

                    gfx.DrawString("Book Events Report", titleFont, XBrushes.Black, 
                        new XRect(margin, 50, usableWidth, 30), XStringFormats.TopLeft);

                    var yPosition = 100;
                    var filterInfo = new List<string>();
                    
                    if (filter.StartDate.HasValue)
                        filterInfo.Add($"Period: {filter.StartDate.Value:d} - {filter.EndDate?.ToString("d") ?? "present"}");
                    if (filter.AccountId.HasValue)
                        filterInfo.Add($"User ID: {filter.AccountId}");
                    if (!string.IsNullOrEmpty(filter.UserName))
                        filterInfo.Add($"User: {filter.UserName}");
                    if (filter.BookId.HasValue)
                        filterInfo.Add($"Book ID: {filter.BookId}");
                    if (!string.IsNullOrEmpty(filter.BookTitle))
                        filterInfo.Add($"Book: {filter.BookTitle}");
                    if (filter.EventTypeId.HasValue)
                        filterInfo.Add($"Event Type ID: {filter.EventTypeId}");
                    if (!string.IsNullOrEmpty(filter.EventTypeName))
                        filterInfo.Add($"Event Type: {filter.EventTypeName}");
                    if (!string.IsNullOrEmpty(filter.AuthorName))
                        filterInfo.Add($"Author: {filter.AuthorName}");
                    if (!string.IsNullOrEmpty(filter.CategoryName))
                        filterInfo.Add($"Category: {filter.CategoryName}");
                    if (!string.IsNullOrEmpty(filter.BranchName))
                        filterInfo.Add($"Branch: {filter.BranchName}");
                    
                    if (filterInfo.Any())
                    {
                        gfx.DrawString("Filters applied:", boldFont, XBrushes.Black, margin, yPosition);
                        yPosition += 20;
                        
                        foreach (var info in filterInfo)
                        {
                            gfx.DrawString(info, font, XBrushes.Black, margin + 20, yPosition);
                            yPosition += 20;
                        }
                    }
                    else
                    {
                        gfx.DrawString("No filters applied - showing all events", font, XBrushes.Black, margin, yPosition);
                        yPosition += 20;
                    }

                    yPosition += 20;

                    var headerRect = new XRect(margin, yPosition - 5, usableWidth, 25);
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(240, 240, 240)), headerRect);

                    var headers = new[] { "Date", "Book Title", "User", "Event Type", "Branch" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        var headerRect2 = new XRect(columnX[i], yPosition, columnWidths[i], 20);
                        gfx.DrawString(headers[i], boldFont, XBrushes.Black, headerRect2, XStringFormats.TopLeft);
                    }

                    yPosition += 25;

                    foreach (var evt in events)
                    {
                        if (yPosition > page.Height - 50)
                        {
                            page = document.AddPage();
                            page.Size = PdfSharp.PageSize.A4;
                            page.Orientation = PdfSharp.PageOrientation.Landscape;
                            gfx = XGraphics.FromPdfPage(page);
                            yPosition = 50;
                        }

                        var rowRect = new XRect(margin, yPosition - 5, usableWidth, 25);
                        if (events.IndexOf(evt) % 2 == 0)
                        {
                            gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(252, 252, 252)), rowRect);
                        }

                        var dateRect = new XRect(columnX[0], yPosition, columnWidths[0], 20);
                        var titleRect = new XRect(columnX[1], yPosition, columnWidths[1], 20);
                        var userRect = new XRect(columnX[2], yPosition, columnWidths[2], 20);
                        var eventRect = new XRect(columnX[3], yPosition, columnWidths[3], 20);
                        var branchRect = new XRect(columnX[4], yPosition, columnWidths[4], 20);

                        gfx.DrawString(evt.event_date.ToString("dd.MM.yyyy"), font, XBrushes.Black, dateRect, XStringFormats.TopLeft);
                        gfx.DrawString(TruncateString(evt.book_title, titleRect, font, gfx), font, XBrushes.Black, titleRect, XStringFormats.TopLeft);
                        gfx.DrawString(TruncateString(evt.user_name, userRect, font, gfx), font, XBrushes.Black, userRect, XStringFormats.TopLeft);
                        gfx.DrawString(TruncateString(evt.event_type_name, eventRect, font, gfx), font, XBrushes.Black, eventRect, XStringFormats.TopLeft);
                        gfx.DrawString(TruncateString(evt.branch_name, branchRect, font, gfx), font, XBrushes.Black, branchRect, XStringFormats.TopLeft);

                        yPosition += 25;
                    }

                    using (var ms = new MemoryStream())
                    {
                        document.Save(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating PDF: {ex.Message}");
            }
        }

        private string TruncateString(string text, XRect rect, XFont font, XGraphics gfx)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var size = gfx.MeasureString(text, font);
            if (size.Width <= rect.Width) return text;

            string ellipsis = "...";
            int length = text.Length;
            while (length > 0)
            {
                string truncated = text.Substring(0, length) + ellipsis;
                size = gfx.MeasureString(truncated, font);
                if (size.Width <= rect.Width) return truncated;
                length--;
            }

            return ellipsis;
        }

        public async Task<List<BookEventListDTO>> GetFilteredEventsAsync(BookEventFilterDTO filter)
        {
            try
            {
                filter ??= new BookEventFilterDTO();
                
                var conditions = new List<string>();
                var parameters = new DynamicParameters();

                if (filter.AccountId.HasValue)
                {
                    conditions.Add("be.account_id = @AccountId");
                    parameters.Add("AccountId", filter.AccountId.Value);
                }

                if (filter.BookId.HasValue)
                {
                    conditions.Add("be.book_id = @BookId");
                    parameters.Add("BookId", filter.BookId.Value);
                }

                if (filter.EventTypeId.HasValue)
                {
                    conditions.Add("be.event_type_id = @EventTypeId");
                    parameters.Add("EventTypeId", filter.EventTypeId.Value);
                }

                if (filter.StartDate.HasValue)
                {
                    conditions.Add("be.event_date >= @StartDate");
                    parameters.Add("StartDate", filter.StartDate.Value);
                }

                if (filter.EndDate.HasValue)
                {
                    conditions.Add("be.event_date <= @EndDate");
                    parameters.Add("EndDate", filter.EndDate.Value);
                }

                if (!string.IsNullOrEmpty(filter.BookTitle))
                {
                    conditions.Add("b.title ILIKE @BookTitle");
                    parameters.Add("BookTitle", $"%{filter.BookTitle}%");
                }

                if (!string.IsNullOrEmpty(filter.UserName))
                {
                    conditions.Add("a.full_name ILIKE @UserName");
                    parameters.Add("UserName", $"%{filter.UserName}%");
                }

                if (!string.IsNullOrEmpty(filter.EventTypeName))
                {
                    conditions.Add("et.name ILIKE @EventTypeName");
                    parameters.Add("EventTypeName", $"%{filter.EventTypeName}%");
                }

                if (!string.IsNullOrEmpty(filter.AuthorName))
                {
                    conditions.Add("au.full_name ILIKE @AuthorName");
                    parameters.Add("AuthorName", $"%{filter.AuthorName}%");
                }

                if (!string.IsNullOrEmpty(filter.CategoryName))
                {
                    conditions.Add("c.name ILIKE @CategoryName");
                    parameters.Add("CategoryName", $"%{filter.CategoryName}%");
                }

                if (!string.IsNullOrEmpty(filter.BranchName))
                {
                    conditions.Add("br.name ILIKE @BranchName");
                    parameters.Add("BranchName", $"%{filter.BranchName}%");
                }

                var whereClause = conditions.Any() ? $"WHERE {string.Join(" AND ", conditions)}" : string.Empty;

                const string baseSql = @"
                    SELECT 
                        be.id as bookevent_id,
                        be.book_id,
                        b.title as book_title,
                        be.account_id,
                        a.full_name as user_name,
                        et.name as event_type_name,
                        be.event_date,
                        au.full_name as author_name,
                        c.name as category_name,
                        br.name as branch_name
                    FROM book_events be
                    JOIN books b ON b.id = be.book_id
                    JOIN accounts a ON a.id = be.account_id
                    JOIN authors au ON au.id = b.author_id
                    JOIN categories c ON c.id = b.category_id
                    JOIN branches br ON br.id = b.branch_id
                    JOIN event_types et ON et.id = be.event_type_id
                    {0}
                    ORDER BY be.event_date DESC";

                var sql = string.Format(baseSql, whereClause);
                var events = await DbConnect.QueryAsync<BookEventListDTO>(sql, parameters);
                return events.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving events: {ex.Message}");
            }
        }
    }
} 