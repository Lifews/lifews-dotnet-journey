// Serilog 基于文件的配置

#if DEBUG

// 构建配置
using Destructurama.Attributed;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using SerilogTimings;
using SerilogTimings.Extensions;
using System.IO;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 从json配置创建Logger
ILogger logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    //.WriteTo.Console(new JsonFormatter()) // 以Json格式输出
    .Destructure.ByTransforming<Payment>(x => new { x.UserId, x.OccuredAt }) // 只留下该结构体中的某些属性，当然也可以做逻辑处理
    .CreateLogger();

// 关于用静态类还是对象的问题
// 工业应用可以直接用静态类吧，Web端再考虑用依赖注入（暂时先这样
// 下面都先用静态类做日志记录
Log.Logger = logger;
logger.Information("程序启动");

var _payment = new Payment()
{
    PaymentId = 201930362019,
    UserId = Guid.NewGuid(),
    OccuredAt = DateTime.UtcNow,
};

// 在结构化数据前加@，会自动序列化对象
// 后续研究一下怎么让输出的格式阅读性更强一点
Log.Information("结构化数据前加@，会自动序列化对象，像这样: {@Payment}", _payment);

using (LogContext.PushProperty("{PaymentId}", _payment.PaymentId))
{
    Log.Information("用户支付Id {UserId}  ", _payment.UserId);
    // 以json格式输出才可以看到
}

using (
    var op = Operation.Begin("日志计时功能演示：{PaymentId} 的支付请求完成 ", _payment.PaymentId)
)
{
    // 执行耗时操作
    await Task.Delay(1000);
    op.Complete(); // 标记操作成功完成
}

class Payment
{
    [LogMasked(ShowFirst = 3, PreserveLength = true)] // 保护敏感数据
    public long PaymentId { get; set; }
    public Guid UserId { get; set; }
    public DateTime OccuredAt { get; set; }
}

// 日志记录的初始化示例
class APP
{
    void InitLog()
    {
        var LOG_TEMPLATE =
            @"[{Timestamp:yyyy-MM-dd hh:mm:ss.fff} {Level:u3}] {Message:lj} {NewLine}{Exception}";
        //var _routerSink = new UISink(new ObservableCollection<string>());
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext() // 从日志上下文中获取属性
            .WriteTo.File(
                path: "Logs/log-.txt", // 日志前缀
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: false,
                shared: true,
                outputTemplate: LOG_TEMPLATE
            )
            .WriteTo.Console(outputTemplate: LOG_TEMPLATE, theme: AnsiConsoleTheme.Code)
            .CreateLogger();
    }
}


#endif
