namespace Backend.DTOs.Book
{
    public class RemoveBookFromBooklistRequest
    {
        public int BooklistId { get; set; }
        public string ISBN { get; set; } = string.Empty;
    }
}
