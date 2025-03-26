namespace BooksApi.DTO.EventBook
{
    public class SavedBookDetailsDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public required string BookTitle { get; set; }
        public required string CoverLink { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 