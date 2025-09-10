// 文件: backend/DTOs/Admin/UpdateLibrarianDto.cs
// 这个 DTO 用于接收前端传来用于“修改”管理员的数据
namespace library_system.DTOs.Admin {
    public class UpdateLibrarianDto {
        public string Name { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
    }
}