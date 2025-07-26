using backend.DTOs.Web;
using backend.Services.ReaderService;
using backend.Services.Web;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.LoginController
{

    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly ReaderService _readerService;

        public LoginController(LoginService loginService, ReaderService readerService)
        {
            _loginService = loginService;
            _readerService = readerService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
        {
            return Ok(await _loginService.LoginAsync(loginDto));
        }

        //注册
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDto registerDto)
        {
            return Ok(await _readerService.RegisterReaderAsync(registerDto));
        }
    }
}
