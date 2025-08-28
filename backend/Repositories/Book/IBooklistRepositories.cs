using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DTOs.Book;

namespace Backend.Repositories.Book
{
    public interface IBooklistRepository
    {
        Task<CreateBooklistResponse> CreateBooklistAsync(string name, string? intro, int creatorId);
        Task<BooklistSuccessResponse> DeleteBooklistAsync(int booklistId, int readerId);
        Task<BooklistSuccessResponse> AddBookToBooklistAsync(int booklistId, string isbn, string? notes);
        Task<BooklistSuccessResponse> RemoveBookFromBooklistAsync(int booklistId, string isbn);
        Task<BooklistSuccessResponse> CollectBooklistAsync(int booklistId, int readerId, string? notes);
        Task<BooklistSuccessResponse> CancelCollectBooklistAsync(int booklistId, int readerId);
        Task<GetBooklistDetailsResponse> GetBooklistDetailsAsync(int booklistId);
        Task<RecommendBooklistsResponse> RecommendBooklistsAsync(int booklistId, int limit = 10);
        Task<SearchBooklistsByReaderResponse> SearchBooklistsByReaderAsync(int readerId);

        Task<BooklistSuccessResponse> UpdateBooklistNameAsync(int booklistId, int readerId, string newName);
        Task<BooklistSuccessResponse> UpdateBooklistIntroAsync(int booklistId, int readerId, string newIntro);
        Task<BooklistSuccessResponse> UpdateCollectNotesAsync(int booklistId, int readerId, string newNotes);
    }
}