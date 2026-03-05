// 比较高明的用日志来捕获节拍耗时的方法

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Demo.LoggingInWpf.Program4;

public class TimedOperation : IDisposable
{
    private readonly ILogger _logger;
    private readonly LogLevel _level;
    private readonly string _messageTemplate;
    private readonly object?[] _args;
    private readonly long _startTimestamp;

    public TimedOperation(ILogger logger, LogLevel level, string messageTemplate, object?[] args)
    {
        _logger = logger;
        _level = level;
        _messageTemplate = messageTemplate;
        _args = new object[args.Length + 1];
        Array.Copy(args, _args, args.Length);
        _startTimestamp = Stopwatch.GetTimestamp();
    }

    public void Dispose()
    {
        TimeSpan delta = Stopwatch.GetElapsedTime(_startTimestamp); // 传入起始时间戳，返回一个 TimeSpan 对象，表示从起始时间到现在经过的时间间隔
        _args[^1] = delta.TotalMilliseconds;
        // C# 8.0 引入的 "System.Index" 和 "hat" 操作符
        // _args[^1]   // 最后一个元素
        // _args[^2]   // 倒数第二个元素
        // _args[^3]   // 倒数第三个元素
        _logger.Log(_level, $"{_messageTemplate} completed in {{OperationDurationMs}}ms", _args); // 双重花括号 {{ }} 是为了在字符串插值中输出单花括号
    }
}
