// Serilog的简单使用

#if !DEBUG

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

#region 基础用法

var LOG_TEMPLATE =
    @"[{Timestamp:yyyy-MM-dd hh:mm:ss.fff} {Level:u3}] {Message:lj} {NewLine}{Exception}";

ILogger logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.File(
        path: "Logs/log-.txt", // 日志前缀
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: false,
        shared: true,
        outputTemplate: LOG_TEMPLATE
    )
    .CreateLogger();

Serilog.Log.Logger = logger;

logger.Information("程序启动");

#endregion

#endif
