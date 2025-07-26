// 文件: backend/Program.cs
// 这是集成了您的原有功能和我们新增的“管理员服务”的完整代码

// ===================================================================
// 1. using 声明区域 (我们新增了 Dapper 的命名空间)
// ===================================================================
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Data; // 您原有的 EF Core 上下文
using Microsoft.OpenApi.Models;
// --- 新增的服务/仓库的命名空间 ---
using library_system.Repositories.Admin;
using library_system.Services.Admin;


// ===================================================================
// 2. WebApplication Builder 初始化和配置
// ===================================================================
var builder = WebApplication.CreateBuilder(args);

// --- 加载 .env 文件 ---
Env.Load();

// --- 读取环境变量 ---
string dbHost    = Environment.GetEnvironmentVariable("DB_HOST")     ?? "localhost";
string dbPort    = Environment.GetEnvironmentVariable("DB_PORT")     ?? "1521";
string dbSvc     = Environment.GetEnvironmentVariable("DB_SERVICE")  ?? "ORCLPDB1";
string dbUser    = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "lib_admin";
string dbPass    = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "dev-secret";
string corsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS") ?? "http://localhost:5173";

// --- 拼接 Oracle 连接串 ---
// string connString = $"User Id={dbUser};Password={dbPass};Data Source={dbHost}:{dbPort}/{dbSvc}";


// ===================================================================
// 3. 依赖注入 (DI) 容器配置 - 服务注册区域
// ===================================================================

// --- 【临时诊断步骤】 ---
// 我们暂时不从环境变量读取，而是直接硬编码一个确认可用的连接字符串。
// 请确保这里的 User Id 和 Password 与您在 SQL Developer 中测试成功的一致。
// 正确的、无需任何引号的硬编码连接字符串
string connString = "User Id=final_owner;Password=Sjk202507;Data Source=115.190.151.58:1521/orclpdb1;";

// --- 检查硬编码的字符串 ---
Console.WriteLine("==========================================================");
Console.WriteLine("正在使用硬编码的连接字符串进行连接测试...");
Console.WriteLine($"连接串: {connString}");
Console.WriteLine("==========================================================");

// --- 注册 EF Core Oracle 数据库上下文 (您原有的) ---
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseOracle(connString, b => b.UseOracleSQLCompatibility("11"))
);

// --- 注册控制器和 Swagger (您原有的) ---
builder.Services.AddControllers();

// 【核心修正】在 AddControllers 后面追加 AddJsonOptions 配置
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // 配置 JSON 序列化器，使其自动将属性名转换为 camelCase
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- ↓↓↓ 我们新增的服务注册代码 ↓↓↓ ---
// 为“管理员增删改查”功能注册 Repository 和 Service。
// 我们使用 Dapper，所以这里是独立于 EF Core 的。

// 1. 注册“管理员增删改查”功能所需的服务
// 【注意】这一部分很可能是您之前缺失的！
builder.Services.AddSingleton(new AdminRepository(connString));
builder.Services.AddTransient<AdminService>();

// 2. 注册“公告发布”功能所需的服务
builder.Services.AddSingleton(new library_system.Repositories.Admin.AnnouncementRepository(connString));
builder.Services.AddTransient<library_system.Services.Admin.AnnouncementService>();
// --- JWT 身份验证 (您原有的) ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = false,
            ValidateAudience         = false,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

// --- CORS 设置 (您原有的) ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins.Split(';'))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// ===================================================================
// 4. 构建 WebApplication 实例
// ===================================================================
var app = builder.Build();


// ===================================================================
// 5. HTTP 请求处理管道配置 - 中间件区域
// ===================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();                  // 启用 CORS
app.UseAuthentication();        // 启用身份验证
app.UseAuthorization();         // 启用授权

app.MapControllers();           // 映射控制器路由

// ===================================================================
// 6. 运行应用
// ===================================================================


app.Run();