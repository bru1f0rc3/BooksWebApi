namespace WebApplication2.DTO.BookEvent
{
    public class BookEventFilterDTO
    {
        public int? AccountId { get; set; }
        public int? BookId { get; set; }
        public int? EventTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? BookTitle { get; set; }
        public string? UserName { get; set; }
        public string? EventTypeName { get; set; }
        public string? AuthorName { get; set; }
        public string? CategoryName { get; set; }
        public string? BranchName { get; set; }
    }
} 