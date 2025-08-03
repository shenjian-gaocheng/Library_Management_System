using System.ComponentModel.DataAnnotations;
namespace library_system.DTOs.Admin
{
    public class CreateOrUpdateAnnouncementDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public string TargetGroup { get; set; } = string.Empty; // "所有人", "读者"
        [Required]
        public string Status { get; set; } = string.Empty; // "发布中", "已撤回"
        
        // 创建时，LibrarianID 应从当前登录用户的 Token 中获取，这里暂时简化
        [Required]
        public string LibrarianID { get; set; } = string.Empty;
    }
}