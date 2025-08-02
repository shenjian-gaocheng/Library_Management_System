using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class BookShelfController : ControllerBase
{
    private readonly BookShelfService _service;

    public BookShelfController(BookShelfService service)
    {
        _service = service;
    }



    // 新增独立搜索功能
    [HttpGet("search_book_which_shelf")]
    public async Task<IEnumerable<BookDto>> SearchBookWhichShelf(
        string keyword)
    {
        return await _service.SearchBookWhichShelfAsync(keyword ?? "");
    }
}