using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.DTOs.Admin;
using backend.Services.Admin;
using System;
using System.Collections.Generic; // 确保引入

namespace backend.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/reports")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<ReportDetailDto>>> GetPendingReports()
        {
            var reports = await _service.GetPendingReportsAsync();
            return Ok(reports);
        }
        
        [HttpPut("{reportId}")]
        public async Task<IActionResult> HandleReport(int reportId, [FromBody] HandleReportDto dto)
        {
            try
            {
                // In a real app, adminId would come from JWT claims.
                var adminId = 1; // Placeholder
                var success = await _service.HandleReportAsync(reportId, dto.Action, dto.BanUser, adminId);
                if (success)
                {
                    return NoContent();
                }
                return BadRequest("Failed to handle the report.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}