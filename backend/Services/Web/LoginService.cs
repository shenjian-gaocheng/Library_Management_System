using backend.Common.Constants;
using backend.DTOs.Web;
using backend.Models;
using backend.Repositories.ReaderRepository;

namespace backend.Services.Web
{
    public class LoginService
    {
        private readonly ReaderRepository _readerRepository;
        private readonly TokenService _tokenService;
        private readonly SecurityService _securityService;
        public LoginService(ReaderRepository readerRepository, TokenService tokenService, SecurityService securityService)
        {
            _readerRepository = readerRepository;
            _tokenService = tokenService;
            _securityService = securityService;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            string userName = loginDto.UserName;
            string password = loginDto.Password;

            //验证码校验
            ValidateCaptha();
            //前置校验，检查用户名和密码是否合法（为空或长度不符合要求）
            PreCheck(userName, password);

            //尝试从数据库中获取用户信息
            //var reader = _readerRepository.GetByUserNameAsync(userName);
            //
            var reader = _readerRepository.GetByIDAsync(userName);// 这里假设用户名即为ReaderID

            if (reader.Result == null)
            {
                throw new ArgumentException("用户名不存在");
            }
            else if (reader.Result.AccountStatus == "冻结")
            {
                throw new ArgumentException("账户已被冻结，请联系管理员");
            }

            //检查密码是否匹配
            _securityService.VerifyPassword(password, reader.Result.Password);

            //创建LoginUser对象
            LoginUser loginUser = new LoginUser(reader.Result);

            //生成token并存入到Redis中
            return await _tokenService.CreateTokenAsync(loginUser);
        }

        //验证码校验
        private void ValidateCaptha()
        {

        }

        //前置校验，检查用户名和密码是否合法（为空或长度不符合要求）
        private void PreCheck(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("用户名或密码不能为空");
            }
            if (userName.Length < UserConstants.USERNAME_MIN_LENGTH || userName.Length > UserConstants.USERNAME_MAX_LENGTH)
            {
                throw new ArgumentException("用户名长度必须在2到20个字符之间");
            }
            if (password.Length < UserConstants.PASSWORD_MIN_LENGTH || password.Length > UserConstants.PASSWORD_MAX_LENGTH)
            {
                throw new ArgumentException("密码长度必须在5到20个字符之间");
            }
        }
    }
}

