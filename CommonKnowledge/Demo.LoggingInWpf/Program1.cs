// 最基础的ILogger,日志记录的核心
// 直接创建LoggerFactory并配置提供程序

#if !DEBUG

using System.Text.Json;
using Microsoft.Extensions.Logging;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    // 移除所有当前已注册的日志提供程序
    builder.ClearProviders();

    builder.AddConsole();

    //builder.AddDebug();

    //builder.AddJsonConsole(options =>
    //{
    //    options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions { Indented = true };
    //});
    //builder.SetMinimumLevel(LogLevel.Information);

    //builder.AddFilter("Program", LogLevel.Warning);
});

ILogger logger = loggerFactory.CreateLogger<Program>();

#region 基本用法

var name = "LiHua";
var age = 30;

// 注意，请使用结构化日志，避免使用字符串拼接，这样可以优化性能

logger.LogDebug(5001, "{Name} just turned : {Age}", name, age);
logger.LogInformation(3001, "{Name} just turned : {Age}", name, age);
logger.LogWarning(1001, "{Name} just turned : {Age}", name, age);

int paymentId = 1;
decimal amount = 15.99m;
DateTime date = DateTime.Now;

logger.LogInformation(
    "New Payment with id {paymentId} for {amount:c} at {date:yyyy-MM-dd HH:mm:ss.fff}",
    paymentId,
    amount,
    date
);

#endregion

#region 处理对象

var _paymentData1 = new PaymentData1 { Amount = 15.99m, PaymentId = 1 };

var _paymentData2 = new PaymentData2 { Amount = 15.99m, PaymentId = 1 };

var _paymentData3 = new PaymentData3 { Amount = 15.99m, PaymentId = 1 };

logger.LogInformation("New Payment with data {PaymentData}", _paymentData1);

logger.LogInformation("New Payment with data {PaymentData}", _paymentData2);

logger.LogInformation(
    "New Payment with data {PaymentData}",
    JsonSerializer.Serialize(_paymentData3)
);

class PaymentData1
{
    public decimal Amount { get; set; }
    public int PaymentId { get; set; }

    // 这样并不高明，如果想要真正记录下类的结构化信息，应该使用record
    // 但是你不可能在所有地方都能使用record，这样的话可以用序列化
    public override string ToString()
    {
        return $"PaymentId:{PaymentId}, Amount:{Amount}";
    }
}

record PaymentData2
{
    public decimal Amount { get; set; }
    public int PaymentId { get; set; }
}

class PaymentData3
{
    public decimal Amount { get; set; }
    public int PaymentId { get; set; }
}

#endregion


#endif
