namespace Backend.DTOs.Book
{
    public class CancelCollectBooklistRequest
    {
        public int BooklistId { get; set; }
        public int ReaderId { get; set; }
    }
}