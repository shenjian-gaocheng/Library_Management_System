using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Admin
{
    public class HandleReportDto
    {
        [Required]
        [RegularExpression("^(approve|reject)$")]
        public string Action { get; set; } = string.Empty;

        public bool BanUser { get; set; } = false; // 新增：是否禁言用户
    }
}