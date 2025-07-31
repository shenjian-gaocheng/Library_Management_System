namespace backend.Common.MiddleWare
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // 配置不需要验证的路径
        private static readonly string[] _excludedPaths = new[]
        {
        "/api/login",
        "/api/register",
        "/api/docs",
        "/api/docs/index.html",
        "/favicon.ico",
        "/api"
    };


        public async Task Invoke(HttpContext context, TokenService tokenService) {

            var path = context.Request.Path.Value;

            // 判断是否跳过认证
            if (_excludedPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context); // 直接放行
                return;
            }

            var loginUser = await tokenService.GetLoginUserAsync(context);
            if (loginUser != null)
            {
                context.Items["LoginUser"] = loginUser;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // 未授权
                return;
            }
            await _next(context); // 调用下一个中间件
        }
    }
}
