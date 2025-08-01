using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly BookCategoryService _service;

    public CategoryController(BookCategoryService service)
    {
        _service = service;
    }

    // 查询分类树
    [HttpGet("tree")]
    public async Task<ActionResult<List<CategoryNode>>> GetTree()
    {
        var tree = await _service.GetCategoryTreeAsync();
        return Ok(tree);
    }

    // 添加分类
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CategoryRequest request)
    {
        try
        {
            var result = await _service.AddCategoryAsync(request.Category, request.OperatorId);
            if (result)
                return Ok(new { message = "添加成功" });
            return BadRequest(new { message = "添加失败" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 修改分类
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CategoryRequest request)
    {
        try
        {
            var result = await _service.UpdateCategoryAsync(request.Category, request.OperatorId);
            if (result)
                return Ok(new { message = "修改成功" });
            return BadRequest(new { message = "修改失败" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 删除分类
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(string categoryId, [FromQuery] string operatorId)
    {
        try
        {
            var result = await _service.DeleteCategoryAsync(categoryId, operatorId);
            if (result)
                return Ok(new { message = "删除成功" });
            return BadRequest(new { message = "删除失败" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}