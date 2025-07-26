using backend;

var builder = WebApplication.CreateBuilder(args);

// 输出当前环境
Console.WriteLine($"当前运行环境: {builder.Environment.EnvironmentName}");

// 日志配置
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// 添加 Startup 并手动调用其方法
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// 中间件配置
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Startup>();
startup.Configure(app, app.Environment, logger);

// 启动应用
app.Run();
