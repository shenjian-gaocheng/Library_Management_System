using library_system.DTOs.Admin;
using library_system.Services.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/admin")] // 将路由统一为 /api/admin
public class AdminController : ControllerBase
{
    private readonly AdminService _service;

    public AdminController(AdminService service)
    {
        _service = service;
    }

    [HttpGet] // GET /api/admin
    public async Task<ActionResult<IEnumerable<LibrarianDto>>> GetAll()
    {
        var librarians = await _service.GetAllLibrariansAsync();
        return Ok(librarians);
    }

[HttpPost]
public async Task<ActionResult<LibrarianDto>> Create(CreateLibrarianDto dto)
{
    try
    {
        var created = await _service.CreateLibrarianAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = created.LibrarianID }, created);
    }
    catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 1) // ORA-00001 的错误号是 1
    {
        // 如果捕获到的是主键冲突异常
        // 返回一个 409 Conflict 状态码，这比 500 更精确地描述了问题
        return Conflict($"创建失败：ID为 '{dto.LibrarianID}' 的管理员已存在。");
    }
    // 如果是其他未知异常，它仍然会被默认的错误处理中间件捕获并记录为 fail
}

    [HttpPut("{id}")] // PUT /api/admin/A001
    public async Task<IActionResult> Update(string id, UpdateLibrarianDto librarianDto)
    {
        var success = await _service.UpdateLibrarianAsync(id, librarianDto);
        if (!success) return NotFound("未找到指定ID的管理员");
        return NoContent(); // HTTP 204: 操作成功，无返回内容
    }

    [HttpDelete("{id}")] // DELETE /api/admin/A001
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _service.DeleteLibrarianAsync(id);
        if (!success) return NotFound("未找到指定ID的管理员");
        return NoContent();
    }
}