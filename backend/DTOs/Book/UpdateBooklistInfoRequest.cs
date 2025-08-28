namespace Backend.DTOs.Book
{
    public class UpdateBooklistNameRequest
    {
        public int BooklistId { get; set; }
        public int ReaderId { get; set; }
        public string NewName { get; set; } = string.Empty;
    }

    public class UpdateBooklistIntroRequest
    {
        public int BooklistId { get; set; }
        public int ReaderId { get; set; }
        public string? NewIntro { get; set; }
    }
}