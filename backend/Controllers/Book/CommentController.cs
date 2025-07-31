using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("search")]
    public async Task<IEnumerable<CommentDetailDto>> Search(string ISBN)
    {
        return await _commentService.SearchCommentAsync(ISBN ?? "");
    }
}