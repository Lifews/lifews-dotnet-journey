using Microsoft.Extensions.Logging;

namespace Demo.LoggingInWpf.Program4;

public static class TimeOperationExtensions
{
    public static IDisposable BeginTimedOperation(
        this ILogger logger,
        string messageTemplate,
        params object[] args
    )
    {
        return logger.BeginTimedOperation(LogLevel.Information, messageTemplate, args);
    }

    public static IDisposable BeginTimedOperation(
        this ILogger logger,
        LogLevel logLevel,
        string messageTemplate,
        params object[] args
    )
    {
        return new TimedOperation(logger, logLevel, messageTemplate, args);
    }
}
