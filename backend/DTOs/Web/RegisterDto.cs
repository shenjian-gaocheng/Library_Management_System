using Newtonsoft.Json;

namespace backend.DTOs.Web
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
      
        [JsonConstructor] // 序列化特性
        public RegisterDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
