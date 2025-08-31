// 文件: backend/Program.cs
// 这是最终的、使用了最强化的 CORS 命名策略的版本

// ===================================================================
// 1. using 声明区域
// ===================================================================
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Data;
using Microsoft.OpenApi.Models;
using library_system.Repositories.Admin;
using library_system.Services.Admin;

// ===================================================================
// 2. WebApplication Builder 初始化和配置
// ===================================================================
var builder = WebApplication.CreateBuilder(args);

// --- 定义一个 CORS 策略的名称，方便后面使用 ---
var MyCorsPolicy = "_MyCorsPolicy";
// 添加 Swagger 支持
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- 加载 .env 文件 ---
Env.Load();

// --- 读取环境变量 ---
string corsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS") ?? "http://localhost:5173";
string connString = "User Id=final_owner;Password=Sjk202507;Data Source=115.190.151.58:1521/orclpdb1;";
string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "dev-secret";

// ===================================================================
// 3. 依赖注入 (DI) 容器配置 - 服务注册区域
// ===================================================================

// --- 【核心修正】使用一个明确的、带有命名策略的 CORS 配置 ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCorsPolicy,
                      policy =>
                      {
                          policy.WithOrigins(corsOrigins.Split(';'))
                                .AllowAnyHeader()
                                // 明确地告诉服务器，允许 GET, POST, PUT, DELETE 以及预检的 OPTIONS 方法！
                                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
                      });
});

// --- 注册控制器，并配置 JSON 序列化 ---
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// ... (其他服务注册，如 Swagger, DbContext, AdminService 等) ...
builder.Services.AddDbContext<OracleDbContext>(options => options.UseOracle(connString, b => b.UseOracleSQLCompatibility("11")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new AdminRepository(connString));
builder.Services.AddTransient<AdminService>();
builder.Services.AddSingleton(new AnnouncementRepository(connString));
builder.Services.AddTransient<AnnouncementService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(/*...*/);

//书单部分
builder.Services.AddScoped<Backend.Repositories.Book.IBooklistRepository, Backend.Repositories.Book.BooklistRepository>();
builder.Services.AddScoped<Backend.Services.Book.IBooklistService, Backend.Services.Book.BooklistService>();
builder.Services.AddSingleton<IOracleConnectionFactory, OracleConnectionFactory>();

// 注册服务依赖（Repository 使用 Singleton，Service 使用 Transient）
builder.Services.AddSingleton(new BookRepository(connectionString));
builder.Services.AddSingleton(new CommentRepository(connectionString));
builder.Services.AddSingleton(new ReportRepository(connectionString));
builder.Services.AddSingleton(new BookCategoryTreeOperation(connectionString));
builder.Services.AddSingleton(new LogService(connectionString));
builder.Services.AddSingleton(new BookShelfRepository(connectionString));
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<ReportService>();
builder.Services.AddTransient<BookCategoryService>();
builder.Services.AddTransient<BookShelfService>();


// ===================================================================
// 4. 构建 WebApplication 实例
// ===================================================================
var app = builder.Build();

// 启用 Swagger（开发环境）
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 使用 CORS（顺序要在 MapControllers 之前）
app.UseCors();

// 【终极诊断：请求侦测中间件】
// 将这个中间件放在所有其他 app.Use... 的最前面！
// ===================================================================
app.Use(async (context, next) =>
{
    Console.WriteLine("==========================================================");
    Console.WriteLine($"[侦测到请求] 时间: {DateTime.Now}");
    Console.WriteLine($"[侦-测-到-请-求] 方法 (Method): {context.Request.Method}"); // <-- 最关键的信息！
    Console.WriteLine($"[侦测到请求] 路径 (Path): {context.Request.Path}");
    Console.WriteLine($"[侦测到请求] 源 (Origin): {context.Request.Headers["Origin"]}");
    Console.WriteLine("--------------------- 请求头 (Headers) ---------------------");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"  {header.Key}: {header.Value}");
    }
    Console.WriteLine("==========================================================");

    // 将请求传递给管道中的下一个中间件
    await next.Invoke();
});
// ===================================================================
// 5. HTTP 请求处理管道配置 - 中间件区域
// ===================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// 【核心修正】应用我们上面定义的那个命名策略
app.UseCors(MyCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ===================================================================
// 6. 运行应用
// ===================================================================
app.Run();