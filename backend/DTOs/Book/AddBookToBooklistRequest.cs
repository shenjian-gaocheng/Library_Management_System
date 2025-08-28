namespace Backend.DTOs.Book
{
    public class AddBookToBooklistRequest
    {
        public int BooklistId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}