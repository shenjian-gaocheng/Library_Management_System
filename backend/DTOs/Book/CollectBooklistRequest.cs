namespace Backend.DTOs.Book
{
    public class CollectBooklistRequest
    {
        public int BooklistId { get; set; }
        public int ReaderId { get; set; }
        public string? Notes { get; set; }
    }
}
