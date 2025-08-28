using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.DTOs.Book;
using Backend.Services.Book;

namespace Backend.Controllers.Book
{
    [ApiController]
    [Route("api/book/[controller]")]
    public class BooklistsController : ControllerBase
    {
        private readonly IBooklistService _service;

        public BooklistsController(IBooklistService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooklist([FromBody] CreateBooklistRequest request)
        {
            int creatorId = 1; // TODO: 从 token 中获取
            var result = await _service.CreateBooklistAsync(request, creatorId);
            return Ok(result);
        }

        [HttpDelete("{booklistId}")]
        public async Task<IActionResult> DeleteBooklist(int booklistId)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.DeleteBooklistAsync(booklistId, readerId);
            return Ok(result);
        }

        [HttpGet("{booklistId}")]
        public async Task<IActionResult> GetBooklistDetails(int booklistId)
        {
            var result = await _service.GetBooklistDetailsAsync(booklistId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{booklistId}/recommend")]
        public async Task<IActionResult> RecommendBooklists(int booklistId, [FromQuery] int limit = 10)
        {
            var result = await _service.RecommendBooklistsAsync(booklistId, limit);
            return Ok(result);
        }

        [HttpPost("{booklistId}/books")]
        public async Task<IActionResult> AddBookToBooklist(int booklistId, [FromBody] AddBookToBooklistRequest request)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.AddBookToBooklistAsync(booklistId, request, readerId);
            return Ok(result);
        }

        [HttpDelete("{booklistId}/books/{isbn}")]
        public async Task<IActionResult> RemoveBookFromBooklist(int booklistId, string isbn)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.RemoveBookFromBooklistAsync(booklistId, isbn, readerId);
            return Ok(result);
        }

        [HttpPost("{booklistId}/collect")]
        public async Task<IActionResult> CollectBooklist(int booklistId, [FromBody] CollectBooklistRequest request)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.CollectBooklistAsync(booklistId, readerId, request);
            return Ok(result);
        }

        [HttpDelete("{booklistId}/collect")]
        public async Task<IActionResult> CancelCollectBooklist(int booklistId)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.CancelCollectBooklistAsync(booklistId, readerId);
            return Ok(result);
        }

        [HttpPut("{booklistId}/collect/notes")]
        public async Task<IActionResult> UpdateCollectNotes(int booklistId, [FromBody] UpdateCollectNotesRequest request)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.UpdateCollectNotesAsync(booklistId, readerId, request);
            return Ok(result);
        }

        [HttpGet("reader/{readerId}")]
        public async Task<IActionResult> SearchBooklistsByReader(int readerId)
        {
            var result = await _service.SearchBooklistsByReaderAsync(readerId);
            return Ok(result);
        }

        [HttpPut("{booklistId}/name")]
        public async Task<IActionResult> UpdateBooklistName(int booklistId, [FromBody] UpdateBooklistNameRequest request)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.UpdateBooklistNameAsync(booklistId, readerId, request);
            return Ok(result);
        }

        [HttpPut("{booklistId}/intro")]
        public async Task<IActionResult> UpdateBooklistIntro(int booklistId, [FromBody] UpdateBooklistIntroRequest request)
        {
            int readerId = 1; // TODO: 从 token 中获取
            var result = await _service.UpdateBooklistIntroAsync(booklistId, readerId, request);
            return Ok(result);
        }
    }
}