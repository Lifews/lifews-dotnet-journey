using Microsoft.AspNetCore.Mvc;

namespace Demo1.Controllers
{
    [ApiController]
    // 这是一个特性（Attribute），标记这个类是一个API控制器
    // 将[ApiController] 特性应用于控制器类，可以启用以下内置行为：
    //自动 HTTP 400 响应：模型验证失败时，自动返回包含错误详情的 400 Bad Request，无需手动检查 ModelState.IsValid。
    //绑定源推断：不再需要显式使用[FromBody]、[FromQuery] 等特性，框架会根据参数类型和 HTTP 方法自动推断数据来源（例如，复杂类型默认从请求正文绑定）。
    //参数必需性：如果操作参数没有标记为可空（如 string?），且无法从请求中获取，会自动触发验证错误。
    //多部分/表单数据自动推断：对于 IFormFile 类型的参数，自动添加[FromForm]。
    //这些功能让 API 控制器更简洁，减少样板代码。
    [Route("[controller]")]
    // 定义路由模板
    // [Route("[controller]")]中， [controller]会被替换为控制器名（去掉"Controller"后缀）,减少重复和硬编码。
    // [Route("[controller]/[action]"]中， 该控制器下所有动作的默认 URL 路径格式为：/{控制器名}/{动作名}，这种路由模式使 URL 结构清晰，并自动跟随控制器和方法名称的变化，方便维护。
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching",
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")] // 标记这是一个HTTP GET方法，并指定路由名
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                })
                .ToArray();
        }
    }
}
