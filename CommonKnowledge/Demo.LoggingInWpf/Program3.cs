// 检查日志器是否启用

#if !DEBUG

using Microsoft.Extensions.Logging;

ILoggerFactory loggerfactory = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole(x =>
    {
        x.JsonWriterOptions = new System.Text.Json.JsonWriterOptions { Indented = true };
    });
    builder.SetMinimumLevel(LogLevel.Warning);
});

ILogger logger = loggerfactory.CreateLogger<Program>();

var paymentId = 1;
var amount = 15.99;

// 检查日志器是否启用！！！
if (logger.IsEnabled(LogLevel.Information))
{
    // 首先要知道一点，在我们记录日志的时候，会有判断日志器是否启用，来决定日志是否写入提供程序
    // 既然如此？为什么还要检查一遍日志器是否启用？
    // 来看看源码

    /***********
        public static void LogInformation(this ILogger logger, string? message, params object?[] args)
        {
            logger.Log(LogLevel.Information, message, args);
        }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            // 此处检查这个日志级别是否启用
            if (!IsEnabled(logLevel))
            {
                return;
            }
            
            var formattedMessage = formatter(state, exception);
            WriteLog(logLevel, eventId, formattedMessage);
        }
    ***********/

    // params object?[] args 源码中有一个装箱操作，并且参数可能存在计算的需求
    // 可以通过检查日志器是否启用，来避免这些处理逻辑和内存分配，当然要不要做这一步取决于需求
    // 当遇到 参数计算成本高，高频日志 的时候，建议先检查日志器是否启用

    logger.LogInformation("New Payment with id {paymentId} for {amount:c}", paymentId, amount);
}

#endif
