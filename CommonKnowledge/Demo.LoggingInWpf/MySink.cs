// 自定义Sink

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Demo.LoggingInWpf;

public class MySink : ILogEventSink
{
    private readonly IFormatProvider? _formatProvider;

    public MySink(IFormatProvider? formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public MySink()
        : this(null) { }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        Console.WriteLine($"{DateTime.UtcNow} - {message}");
    }
}

public static class MySinkExtensions
{
    public static LoggerConfiguration MySink(
        this LoggerSinkConfiguration sinkConfiguration,
        IFormatProvider? formatProvider = null
    )
    {
        return sinkConfiguration.Sink(new MySink());
    }
}
