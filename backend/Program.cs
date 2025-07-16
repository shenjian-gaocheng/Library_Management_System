using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Data;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// === 加载 .env 文件（开发环境使用）===
Env.Load(); // 加载 .env 文件到系统环境变量

// === 读取环境变量 ===
string dbHost    = Environment.GetEnvironmentVariable("DB_HOST")     ?? "localhost";
string dbPort    = Environment.GetEnvironmentVariable("DB_PORT")     ?? "1521";
string dbSvc     = Environment.GetEnvironmentVariable("DB_SERVICE")  ?? "ORCLPDB1";
string dbUser    = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "lib_admin";
string dbPass    = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "dev-secret";
string corsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS") ?? "http://localhost:5173";

// === 拼接 Oracle 连接串 ===
string connString = $"User Id={dbUser};Password={dbPass};Data Source={dbHost}:{dbPort}/{dbSvc}";

// === 注册 EF Core Oracle 数据库上下文 ===
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseOracle(connString, b => b.UseOracleSQLCompatibility("11"))
);

// === 注册控制器和 Swagger（可选）===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// === JWT 身份验证 ===
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

// === CORS 设置 ===
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins.Split(';'))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// === 中间件 ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();                  // 启用 CORS
app.UseAuthentication();        // 启用身份验证
app.UseAuthorization();         // 启用授权

app.MapControllers();           // 映射控制器路由
app.Run();
