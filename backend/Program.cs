
using backend.Repositories.Admin;
using backend.Services.Admin;

var builder = WebApplication.CreateBuilder(args);

// 输出当前环境（Development / Production）
Console.WriteLine($"当前运行环境: {builder.Environment.EnvironmentName}");

// 添加控制器服务
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// 添加 Swagger 支持
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 添加 CORS 支持（便于前端访问）
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

// 读取连接字符串（根据环境自动读取 appsettings.Development.json 或 appsettings.Production.json）
var connectionString = builder.Configuration.GetConnectionString("OracleDB")
                      ?? throw new InvalidOperationException("缺少 OracleDB 连接字符串配置");

var redisConnStr = builder.Configuration.GetConnectionString("Redis")
                      ?? throw new InvalidOperationException("缺少 Redis 连接字符串配置");

// 注册服务依赖（Repository 使用 Singleton，Service 使用 Transient）
builder.Services.AddSingleton(new BookRepository(connectionString));
builder.Services.AddSingleton(new CommentRepository(connectionString));
builder.Services.AddSingleton(new BookCategoryTreeOperation(connectionString));
builder.Services.AddSingleton(new LogService(connectionString));
builder.Services.AddSingleton(new BookShelfRepository(connectionString));
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<BookCategoryService>();
builder.Services.AddTransient<BookShelfService>();
builder.Services.AddSingleton(new PurchaseAnalysisRepository(connectionString));
builder.Services.AddTransient<PurchaseAnalysisService>();
builder.Services.AddSingleton(new ReportRepository(connectionString));
builder.Services.AddTransient<ReportService>();
builder.Services.AddSingleton(new AnnouncementRepository(connectionString));
builder.Services.AddTransient<AnnouncementService>();
// 注册 Redis 服务（使用连接字符串）
builder.Services.AddSingleton(new RedisService(redisConnStr));

var app = builder.Build();

// 启用 Swagger（开发环境）
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 使用 CORS（顺序要在 MapControllers 之前）
app.UseCors();

// 启用控制器路由
app.UseRouting();
app.MapControllers();

// 启动应用
app.Run();
