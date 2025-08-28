namespace Backend.DTOs.Book
{
    public class UpdateCollectNotesRequest
    {
        public int BooklistId { get; set; }
        public int ReaderId { get; set; }
        public string? NewNotes { get; set; }
    }
}