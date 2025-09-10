using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.DTOs.Book;
using Backend.Services.Book;
using backend.Services.Web;
using backend.Models;

namespace backend.Controllers.Book
{
    [ApiController]
    [Route("api/book/[controller]")]
    public class BooklistsController : ControllerBase
    {
        private readonly IBooklistService _service;

        //用于获取用户id
        private readonly SecurityService _securityService;

        public BooklistsController(IBooklistService service, SecurityService securityService)
        {
            _service = service;
            _securityService = securityService;
        }

        // 获取 ReaderID 的方法
        public long? GetCurrentReaderId()
        {
            var loginUser = _securityService.GetLoginUser();
            
            if (_securityService.CheckIsReader(loginUser))
            {
                var reader = loginUser.User as Reader;
                return reader.ReaderID;
            }
            
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooklist([FromBody] CreateBooklistRequest request)
        {
            long creatorId = GetCurrentReaderId() ?? 0;
            var result = await _service.CreateBooklistAsync(request, (int)creatorId);
            return Ok(result);
        }

        [HttpDelete("{booklistId}")]
        public async Task<IActionResult> DeleteBooklist(int booklistId)
        {
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.DeleteBooklistAsync(booklistId, (int)readerId);
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
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.AddBookToBooklistAsync(booklistId, request, (int)readerId);
            return Ok(result);
        }

        [HttpDelete("{booklistId}/books/{isbn}")]
        public async Task<IActionResult> RemoveBookFromBooklist(int booklistId, string isbn)
        {
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.RemoveBookFromBooklistAsync(booklistId, isbn, (int)readerId);
            return Ok(result);
        }

        [HttpPost("{booklistId}/collect")]
        public async Task<IActionResult> CollectBooklist(int booklistId, [FromBody] CollectBooklistRequest request)
        {
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.CollectBooklistAsync(booklistId, (int)readerId, request);
            return Ok(result);
        }

        [HttpDelete("{booklistId}/collect")]
        public async Task<IActionResult> CancelCollectBooklist(int booklistId)
        {
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.CancelCollectBooklistAsync(booklistId, (int)readerId);
            return Ok(result);
        }

        [HttpPut("{booklistId}/collect/notes")]
        public async Task<IActionResult> UpdateCollectNotes(int booklistId, [FromBody] UpdateCollectNotesRequest request)
        {
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.UpdateCollectNotesAsync(booklistId, (int)readerId, request);
            return Ok(result);
        }

        [HttpGet("reader/{readerId}")]
        public async Task<IActionResult> SearchBooklistsByReader(int readerId)
        {
            long currentReaderId = GetCurrentReaderId() ?? 0;
            var result = await _service.SearchBooklistsByReaderAsync((int)currentReaderId);
            return Ok(result);
        }

        [HttpPut("{booklistId}/name")]
        public async Task<IActionResult> UpdateBooklistName(int booklistId, [FromBody] UpdateBooklistNameRequest request)
        {
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.UpdateBooklistNameAsync(booklistId, (int)readerId, request);
            return Ok(result);
        }

        [HttpPut("{booklistId}/intro")]
        public async Task<IActionResult> UpdateBooklistIntro(int booklistId, [FromBody] UpdateBooklistIntroRequest request)
        {
            long readerId = GetCurrentReaderId() ?? 0;
            var result = await _service.UpdateBooklistIntroAsync(booklistId, (int)readerId, request);
            return Ok(result);
        }
    }
}