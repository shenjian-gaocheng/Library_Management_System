// 文件: backend/Program.cs
// 这是集成了您的原有功能和我们新增的"管理员服务"的完整代码

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

var builder = WebApplication.CreateBuilder(args);

// 输出当前环境（Development / Production）
Console.WriteLine($"当前运行环境: {builder.Environment.EnvironmentName}");

// --- 加载 .env 文件 ---
Env.Load();

// --- 读取环境变量 ---
string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1521";
string dbSvc = Environment.GetEnvironmentVariable("DB_SERVICE") ?? "ORCLPDB1";
string dbUser = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "lib_admin";
string dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "dev-secret";
string corsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS") ?? "http://localhost:5173";

// 使用硬编码的连接字符串进行连接测试
string connString = "User Id=final_owner;Password=Sjk202507;Data Source=115.190.151.58:1521/orclpdb1;";

Console.WriteLine("==========================================================");
Console.WriteLine("正在使用硬编码的连接字符串进行连接测试...");
Console.WriteLine($"连接串: {connString}");
Console.WriteLine("==========================================================");

// 注册 EF Core Oracle 数据库上下文
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseOracle(connString, b => b.UseOracleSQLCompatibility("11"))
);

// 注册控制器和 JSON 序列化选项
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 注册管理员相关服务
builder.Services.AddSingleton(new AdminRepository(connString));
builder.Services.AddTransient<AdminService>();

// 注册公告发布功能所需的服务
builder.Services.AddSingleton(new library_system.Repositories.Admin.AnnouncementRepository(connString));
builder.Services.AddTransient<library_system.Services.Admin.AnnouncementService>();

// JWT 身份验证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// CORS 设置
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()      // 生产环境可替换为具体域名
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 日志输出到控制台（调试用）
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 使用 CORS（顺序要在 MapControllers 之前）
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// 启用控制器路由
app.UseRouting();
app.MapControllers();

// 启动应用
app.Run();