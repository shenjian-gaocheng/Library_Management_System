using backend.DTOs.Web;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly RedisService _redisService;
    private readonly int _expireMinutes;

    public TokenService(IConfiguration config, RedisService redisService)
    {
        _config = config;
        _redisService = redisService;
        _expireMinutes = _config.GetValue<int>("JwtSettings:ExpireMinutes", 30); // 默认30分钟
    }

    public async Task<LoginUser?> GetLoginUserAsync(HttpContext httpContext)
    {
        //var request = _httpContextAccessor.HttpContext?.Request;
        var token = httpContext.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
        {
            if (token.Trim().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Trim().Substring("Bearer ".Length).Trim();
            }
            try
            {
                return await GetLoginUserAsync(token);
            }
            catch (Exception ex)
            {
                // 记录日志，或者处理异常
                // logger.LogError(ex, "获取登录用户失败");
                return null;
            }
        }
        return null;
    }

    public async Task<string> CreateTokenAsync(LoginUser loginUser)
    {
        var token = JwtUtils.CreateToken(loginUser, _config);
        loginUser.Token = token;
        await RefreshTokenAsync(loginUser);
        return token;
    }

    public async Task<LoginUser?> GetLoginUserAsync(string token)
    {
        var redisKey = $"LOGIN_TOKEN:{token}";
        return await _redisService.GetCacheAsync<LoginUser>(redisKey);
    }

    public async Task<bool> DeleteTokenAsync(string token)
    {
        var redisKey = $"LOGIN_TOKEN:{token}";
        return await _redisService.DeleteAsync(redisKey);
    }

    public async Task RefreshTokenAsync(LoginUser loginUser)
    {
        loginUser.LoginTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        loginUser.ExpireTime = loginUser.LoginTime + _expireMinutes * 60;
        await _redisService.SetCacheAsync($"LOGIN_TOKEN:{loginUser.Token}", loginUser, TimeSpan.FromMinutes(_expireMinutes));
    }
}
