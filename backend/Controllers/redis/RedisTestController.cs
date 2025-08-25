using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RedisTestController : ControllerBase
{
    private readonly RedisService _redis;

    public RedisTestController(RedisService redis)
    {
        _redis = redis;
    }

    [HttpGet("set")]
    public IActionResult Set(string key, string value)
    {
        _redis.SetString(key, value);
        return Ok("Redis key set successfully.");
    }

    [HttpGet("get")]
    public IActionResult Get(string key)
    {
        var value = _redis.GetString(key);
        return Ok(value ?? "(nil)");
    }
}

// 测试方法：后端启动后运行http://localhost:5000/api/RedisTest/set?key=foo&value=bar
// 若正常，输出：Redis key set successfully.