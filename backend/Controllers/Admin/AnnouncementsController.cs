using library_system.DTOs.Admin;
using library_system.Services.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_system.Controllers
{
    [ApiController]
    [Route("api/announcements")] // 基础路由
    public class AnnouncementsController : ControllerBase
    {
        private readonly AnnouncementService _service;
        public AnnouncementsController(AnnouncementService service) { _service = service; Console.WriteLine("init announcement"); }

        // 公开接口: GET /api/announcements/public
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetPublic()
        {
            Console.WriteLine("check controller");
            return Ok(await _service.GetPublicAnnouncementsAsync());
        }

        // 管理接口: GET /api/announcements/manage
        [HttpGet("manage")]
        // 在真实项目中，这里应该加上 [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAllForManagement()
        {
            return Ok(await _service.GetAllAnnouncementsAsync());
        }

        // 管理接口: POST /api/announcements/manage
        [HttpPost("manage")]
        public async Task<IActionResult> Create(CreateOrUpdateAnnouncementDto dto)
        {
            var newId = await _service.CreateAnnouncementAsync(dto);
            return Ok(new { announcementId = newId });
        }

        [HttpDelete("manage/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // 您需要在 AnnouncementService 和 AnnouncementRepository 中也添加对应的 DeleteAsync 方法
            var success = await _service.DeleteAnnouncementAsync(id); 
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("manage/{id}")]
        public async Task<IActionResult> Update(int id, CreateOrUpdateAnnouncementDto dto)
        {
            // 确保传入的ID和body里的ID一致或以后者为准
            var success = await _service.UpdateAnnouncementAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}