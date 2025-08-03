using backend.Common.MiddleWare;
using backend.Repositories.ReaderRepository;
using backend.Services.ReaderService;
using backend.Services.Web;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 输出当前环境
Console.WriteLine($"当前运行环境: {builder.Environment.EnvironmentName}");

// 日志配置
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// 添加服务
var services = builder.Services;

// 添加控制器支持
services.AddControllers();

// 添加 CORS 支持
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 添加 Swagger 服务
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "图书馆管理系统 API",
        Version = "v1",
        Description = "基于 ASP.NET Core 的图书馆后台接口文档"
    });
});

// 注册 IHttpContextAccessor
services.AddHttpContextAccessor();

// 注册 Redis
services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrEmpty(configuration))
    {
        throw new ArgumentException("未配置 Redis 连接字符串");
    }
    return ConnectionMultiplexer.Connect(configuration);
});
services.AddSingleton<RedisService>();

// 注册业务服务
services.AddScoped<TokenService>();
services.AddScoped<SecurityService>();
services.AddScoped<LoginService>();

// 注册 ReaderRepository 和 ReaderService
services.AddSingleton(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("OracleDB");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new ArgumentException("未配置 Oracle 数据库连接字符串");
    }
    return new ReaderRepository(connectionString);
});
services.AddTransient<ReaderService>();

// 构建应用
var app = builder.Build();

// 获取 logger
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("StartupLogger");
logger.LogInformation($"当前运行环境: {app.Environment.EnvironmentName}");

// 开发环境启用 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "图书馆 API v1");
        c.RoutePrefix = "api/docs";
    });
}

app.UseStaticFiles(); // 启用 wwwroot 目录下的静态文件

app.UseCors(); // 启用跨域

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>(); // 自定义异常中间件

app.UseMiddleware<JwtAuthenticationMiddleware>(); // JWT 认证中间件

app.UseAuthorization(); // 授权中间件（如果有）

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// 启动应用
app.Run();
