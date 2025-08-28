using System.Threading.Tasks;
using Backend.DTOs.Book;

namespace Backend.Services.Book
{
    public interface IBooklistService
    {
        Task<CreateBooklistResponse> CreateBooklistAsync(CreateBooklistRequest req);
        Task<BooklistSuccessResponse> DeleteBooklistAsync(int booklistId, int readerId);
        Task<BooklistSuccessResponse> AddBookAsync(AddBookToBooklistRequest req);
        Task<BooklistSuccessResponse> RemoveBookAsync(RemoveBookFromBooklistRequest req);
        Task<BooklistSuccessResponse> CollectAsync(CollectBooklistRequest req);
        Task<BooklistSuccessResponse> CancelCollectAsync(CancelCollectBooklistRequest req);
        Task<GetBooklistDetailsResponse> GetDetailsAsync(int booklistId);
        Task<RecommendBooklistsResponse> RecommendAsync(int booklistId, int limit = 10);
        Task<SearchBooklistsByReaderResponse> GetByReaderAsync(int readerId);

        Task<BooklistSuccessResponse> UpdateBooklistNameAsync(UpdateBooklistNameRequest req);
        Task<BooklistSuccessResponse> UpdateBooklistIntroAsync(UpdateBooklistIntroRequest req);
        Task<BooklistSuccessResponse> UpdateCollectNotesAsync(UpdateCollectNotesRequest req);
    }
}