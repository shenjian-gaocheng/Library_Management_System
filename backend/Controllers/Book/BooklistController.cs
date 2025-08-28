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
        public async Task<ActionResult<CreateBooklistResponse>> Create([FromBody] CreateBooklistRequest req)
        {
            var result = await _service.CreateBooklistAsync(req);
            if (result.Success == 1) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{booklistId}")]
        public async Task<ActionResult<BooklistSuccessResponse>> Delete(int booklistId, [FromQuery] int readerId)
        {
            var result = await _service.DeleteBooklistAsync(booklistId, readerId);
            if (result.Success == 1) return Ok(result);
            return Forbid();
        }

        [HttpPost("{booklistId}/books")]
        public async Task<ActionResult<BooklistSuccessResponse>> AddBook(int booklistId, [FromBody] AddBookToBooklistRequest req)
        {
            req.BooklistId = booklistId;
            var result = await _service.AddBookAsync(req);
            return result.Success == 1 ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{booklistId}/books/{isbn}")]
        public async Task<ActionResult<BooklistSuccessResponse>> RemoveBook(int booklistId, string isbn)
        {
            var result = await _service.RemoveBookAsync(new RemoveBookFromBooklistRequest { BooklistId = booklistId, ISBN = isbn });
            return result.Success == 1 ? Ok(result) : NotFound(result);
        }

        [HttpPost("{booklistId}/collect")]
        public async Task<ActionResult<BooklistSuccessResponse>> Collect(int booklistId, [FromBody] CollectBooklistRequest req)
        {
            req.BooklistId = booklistId;
            var result = await _service.CollectAsync(req);
            return result.Success == 1 ? Ok(result) : BadRequest(result);
        }
        
        [HttpDelete("{booklistId}/collect/{readerId}")]
        public async Task<ActionResult<BooklistSuccessResponse>> CancelCollect(int booklistId, int readerId)
        {
            var result = await _service.CancelCollectAsync(new CancelCollectBooklistRequest { BooklistId = booklistId, ReaderId = readerId });
            return result.Success == 1 ? Ok(result) : NotFound(result);
        }

        [HttpGet("{booklistId}")]
        public async Task<ActionResult<GetBooklistDetailsResponse>> Details(int booklistId)
        {
            var result = await _service.GetDetailsAsync(booklistId);
            if (result.BooklistInfo == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{booklistId}/recommendations")]
        public async Task<ActionResult<RecommendBooklistsResponse>> Recommend(int booklistId, [FromQuery] int limit = 10)
        {
            var result = await _service.RecommendAsync(booklistId, limit);
            return Ok(result);
        }

        [HttpGet("readers/{readerId}/booklists")]
        public async Task<ActionResult<SearchBooklistsByReaderResponse>> ByReader(int readerId)
        {
            var result = await _service.GetByReaderAsync(readerId);
            return Ok(result);
        }

        [HttpPatch("{booklistId}/name")]
        public async Task<ActionResult<BooklistSuccessResponse>> UpdateName(int booklistId, [FromBody] UpdateBooklistNameRequest req)
        {
            req.BooklistId = booklistId;
            var result = await _service.UpdateBooklistNameAsync(req);
            return result.Success == 1 ? Ok(result) : Forbid();
        }

        [HttpPatch("{booklistId}/intro")]
        public async Task<ActionResult<BooklistSuccessResponse>> UpdateIntro(int booklistId, [FromBody] UpdateBooklistIntroRequest req)
        {
            req.BooklistId = booklistId;
            var result = await _service.UpdateBooklistIntroAsync(req);
            return result.Success == 1 ? Ok(result) : Forbid();
        }

        [HttpPatch("{booklistId}/collect-notes")]
        public async Task<ActionResult<BooklistSuccessResponse>> UpdateCollectNotes(int booklistId, [FromBody] UpdateCollectNotesRequest req)
        {
            req.BooklistId = booklistId;
            var result = await _service.UpdateCollectNotesAsync(req);
            return result.Success == 1 ? Ok(result) : Forbid();
        }
    }
}