using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Reader;
using backend.Services.ReaderService;
using backend.Services.Web;
using backend.Models;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;

    private readonly SecurityService _securityService;

    public CommentController(CommentService commentService, SecurityService securityService)
    {
        _commentService = commentService;
        _securityService = securityService;
        Console.WriteLine("init comment");
    }

    [HttpGet("search")]
    public async Task<IEnumerable<CommentDetailDto>> Search(string ISBN)
    {
        return await _commentService.SearchCommentAsync(ISBN ?? "");
    }

    [HttpGet("search-id")]
    public async Task<IEnumerable<CommentDetailDto>> Search(decimal id)
    {
        return await _commentService.SearchCommentAsyncByCommentID(id);
    }
    
    [HttpPost("add")]
    public async Task<ActionResult> AddComment([FromBody] CommentDetailDto commentDto)
    {
        var loginUser = _securityService.GetLoginUser();
    
        // 检查登录用户是否为 Reader
        if (_securityService.CheckIsReader(loginUser))
        {
            var reader = loginUser.User as Reader;
            commentDto.ReaderID = reader.ReaderID;
        }
        
        var result = await _commentService.AddCommentAsync(commentDto);
        if (result > 0)
        {
            return Ok(new { Message = "评论添加成功" });
        }
        return BadRequest(new { Message = "评论添加失败" });
    }
}


