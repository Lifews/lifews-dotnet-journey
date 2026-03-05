// 比较高明的实现 计时日志条目

#if !DEBUG

using Demo.LoggingInWpf.Program4;
using Microsoft.Extensions.Logging;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole(options =>
    {
        options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions { Indented = true };
    });
});

ILogger logger = loggerFactory.CreateLogger<Program>();

var paymentId = 201930362019;
var amount = 6850;

using (logger.BeginTimedOperation("Handing new payment"))
{
    logger.LogInformation("New Payment with id {PaymentId} for ${Total}", paymentId, amount);

    await Task.Delay(50);
}

#endif
