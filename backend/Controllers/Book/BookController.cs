using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly BookService _service;

    public BookController(BookService service)
    {
        _service = service;
    }

    [HttpGet("search")]
    public async Task<IEnumerable<BookInfoDto>> Search(string keyword)
    {
        return await _service.SearchBooksAsync(keyword ?? "");
    }
}