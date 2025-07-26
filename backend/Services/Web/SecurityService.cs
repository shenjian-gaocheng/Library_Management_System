using backend.DTOs.Web;

namespace backend.Services.Web
{
    public class SecurityService
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public SecurityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /**
         * 匹配原始密码和加密密码
         * 
         */
        public bool VerifyPassword(string rawPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, hashedPassword);
        }
        /**
         * 加密密码
         * 
         */
        public string HashPassword(string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword))
            {
                throw new ArgumentException("密码不能为空");
            }
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        /**
         * 
         * 从HttpContext中获取登录用户
         */
        public LoginUser? GetLoginUser()
        {
            return _httpContextAccessor?.HttpContext?.Items["LoginUser"] as LoginUser;
        }

        /**
         * 
         * 从HttpContext中获取登录用户名UserName
         */
        public string GetLoginUserName()
        {
            return GetLoginUser().UserName;
        }


    }
}
