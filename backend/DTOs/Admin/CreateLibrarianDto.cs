// 文件: backend/DTOs/Admin/CreateLibrarianDto.cs
// 这个 DTO 用于接收前端传来用于“新增”管理员的数据
using System.ComponentModel.DataAnnotations;
namespace library_system.DTOs.Admin {
    public class CreateLibrarianDto {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        [Required] public string Permission { get; set; } = string.Empty;
    }
}