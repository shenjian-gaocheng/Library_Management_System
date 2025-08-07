// using System.ComponentModel.DataAnnotations; // 这行也可以注释掉

namespace library_system.DTOs.Admin
{
    public class CreateOrUpdateAnnouncementDto
    {
        // [Required] <-- 暂时注释掉
        public string Title { get; set; } = string.Empty;
        // [Required] <-- 暂时注释掉
        public string Content { get; set; } = string.Empty;
        // [Required] <-- 暂时注释掉
        public string TargetGroup { get; set; } = string.Empty;
        // [Required] <-- 暂时注释掉
        public string Status { get; set; } = string.Empty;
        // [Required] <-- 暂时注释掉
        public int LibrarianID { get; set; } // 注意，这个应该是 int
    }
}