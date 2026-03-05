// 日志的作用域
// 允许你在一个逻辑操作或事务中为所有日志消息自动附加相同的上下文信息。

#if !DEBUG

using Microsoft.Extensions.Logging;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole(x =>
    {
        x.IncludeScopes = true;
        x.JsonWriterOptions = new System.Text.Json.JsonWriterOptions { Indented = true };
    });
});

ILogger logger = loggerFactory.CreateLogger<Program>();

var paymentId = 1;
var amount = 15.99;

using (logger.BeginScope("{PaymentID}", paymentId))
{
    try
    {
        logger.LogInformation("New Payment for {amount}", amount);
        // processing
    }
    finally
    {
        logger.LogInformation("Payment processing completed");
    }
}

#endif
