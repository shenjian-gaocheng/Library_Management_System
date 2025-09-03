using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet("search-id")]
    public async Task<IEnumerable<CommentDetailDto>> Search(decimal id)
    {
        return await _commentService.SearchCommentAsyncByCommentID(id);
    }
    
    [HttpPost("add")]
    public async Task<ActionResult> AddComment([FromBody] CommentDetailDto commentDto)
    {
        var result = await _commentService.AddCommentAsync(commentDto);
        if (result > 0)
        {
            return Ok(new { Message = "评论添加成功" });
        }
        return BadRequest(new { Message = "评论添加失败" });
    }
}


[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }
    
    [HttpGet("by-reader/{readerId}")]
    public async Task<ActionResult<IEnumerable<ReportDto>>> GetReportsByReaderId(decimal readerId)
    {
        var reports = await _reportService.GetReportsAsyncByReaderID(readerId);
        return Ok(reports);
    }
    
    [HttpGet("by-librarian/{librarianId}")]
    public async Task<ActionResult<IEnumerable<ReportDto>>> GetReportsByLibrarianId(decimal librarianId)
    {
        var reports = await _reportService.GetReportsAsyncByLibrarianID(librarianId);
        return Ok(reports);
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddReport([FromBody] ReportDto report)
    {
        var result = await _reportService.AddReportAsync(report);
        if (result > 0)
        {
            return Ok(new { Message = "举报添加成功" });
        }
        return BadRequest(new { Message = "举报添加失败" });
    }

    
    [HttpPost("change-status")]
    public async Task<ActionResult> ChangeStatusAsync([FromBody] ReportStatusDto report_status)
    {
        var result = await _reportService.ChangeStatusAsync(report_status);
        if (result > 0)
        {
            return Ok(new { Message = "举报修改成功" });
        }
        return BadRequest(new { Message = "举报修改失败" });
    }
}