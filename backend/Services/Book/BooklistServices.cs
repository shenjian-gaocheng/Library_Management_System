using System;
using System.Threading.Tasks;
using Backend.DTOs.Book;
using Backend.Repositories.Book;

namespace Backend.Services.Book
{
    public class BooklistService : IBooklistService
    {
        private readonly IBooklistRepository _repo;
        public BooklistService(IBooklistRepository repo)
        {
            _repo = repo;
        }

        public async Task<CreateBooklistResponse> CreateBooklistAsync(CreateBooklistRequest req)
        {
            return await _repo.CreateBooklistAsync(req.BooklistName, req.BooklistIntroduction, req.CreatorId);
        }

        public async Task<BooklistSuccessResponse> DeleteBooklistAsync(int booklistId, int readerId)
        {
            return await _repo.DeleteBooklistAsync(booklistId, readerId);
        }

        public async Task<BooklistSuccessResponse> AddBookAsync(AddBookToBooklistRequest req)
        {
            return await _repo.AddBookToBooklistAsync(req.BooklistId, req.ISBN, req.Notes);
        }

        public async Task<BooklistSuccessResponse> RemoveBookAsync(RemoveBookFromBooklistRequest req)
        {
            return await _repo.RemoveBookFromBooklistAsync(req.BooklistId, req.ISBN);
        }

        public async Task<BooklistSuccessResponse> CollectAsync(CollectBooklistRequest req)
        {
            return await _repo.CollectBooklistAsync(req.BooklistId, req.ReaderId, req.Notes);
        }

        public async Task<BooklistSuccessResponse> CancelCollectAsync(CancelCollectBooklistRequest req)
        {
            return await _repo.CancelCollectBooklistAsync(req.BooklistId, req.ReaderId);
        }

        public async Task<GetBooklistDetailsResponse> GetDetailsAsync(int booklistId)
        {
            return await _repo.GetBooklistDetailsAsync(booklistId);
        }

        public async Task<RecommendBooklistsResponse> RecommendAsync(int booklistId, int limit = 10)
        {
            return await _repo.RecommendBooklistsAsync(booklistId, limit);
        }

        public async Task<SearchBooklistsByReaderResponse> GetByReaderAsync(int readerId)
        {
            return await _repo.SearchBooklistsByReaderAsync(readerId);
        }

        public async Task<BooklistSuccessResponse> UpdateBooklistNameAsync(UpdateBooklistNameRequest req)
        {
            return await _repo.UpdateBooklistNameAsync(req.BooklistId, req.ReaderId, req.NewName);
        }

        public async Task<BooklistSuccessResponse> UpdateBooklistIntroAsync(UpdateBooklistIntroRequest req)
        {
            return await _repo.UpdateBooklistIntroAsync(req.BooklistId, req.ReaderId, req.NewIntro);
        }

        public async Task<BooklistSuccessResponse> UpdateCollectNotesAsync(UpdateCollectNotesRequest req)
        {
            return await _repo.UpdateCollectNotesAsync(req.BooklistId, req.ReaderId, req.NewNotes);
        }
    }
}