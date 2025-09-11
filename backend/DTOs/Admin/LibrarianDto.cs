// 文件: backend/DTOs/Admin/LibrarianDto.cs
// 这个 DTO 用于从后端发送管理员信息到前端（用于查询和展示）
namespace library_system.DTOs.Admin {
    public class LibrarianDto {
        public int LibrarianID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
    }
}