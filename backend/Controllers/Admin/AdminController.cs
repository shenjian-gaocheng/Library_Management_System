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
public async Task<ActionResult<LibrarianDto>> Create(CreateLibrarianDto librarianDto)
{
    // CreateLibrarianAsync 现在会返回一个包含了新 ID 的 LibrarianDto 对象
    var createdLibrarian = await _service.CreateLibrarianAsync(librarianDto);
    
    // 【核心修正】
    // 从返回的、已经包含新 ID 的 DTO 对象中获取 LibrarianID
    return CreatedAtAction(nameof(GetAll), new { id = createdLibrarian.LibrarianID }, createdLibrarian);
}

    [HttpPut("{id}")] // PUT /api/admin/A001
    public async Task<IActionResult> Update(int id, UpdateLibrarianDto librarianDto)
    {
        var success = await _service.UpdateLibrarianAsync(id, librarianDto);
        if (!success) return NotFound("未找到指定ID的管理员");
        return NoContent(); // HTTP 204: 操作成功，无返回内容
    }

    [HttpDelete("{id}")] // DELETE /api/admin/A001
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteLibrarianAsync(id);
        if (!success) return NotFound("未找到指定ID的管理员");
        return NoContent();
    }
}