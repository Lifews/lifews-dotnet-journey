# `ToString()`是什么?

在打印对象、调试或记录日志时，经常需要将对象转换为字符串，这时就会调用`ToString()`方法。

在C#中，所有的类都隐式继承自`object`类（除非指定了其他父类）。

`object`类中有一个虚方法ToString()`，因此任何类都可以通过重写（override）这个方法来提供自己的字符串表示。





# `$` 是 什么？

**`$` 是 C# 中的字符串插值（String Interpolation）符号**，是 C# 6.0（2015年）引入的语法糖。

```csharp
// 传统方式1：字符串拼接
public override string ToString()
{
    return "PaymentId:" + _paymentId + ", Amount:" + _amount;
}

// 传统方式2：string.Format
public override string ToString()
{
    return string.Format("PaymentId:{0}, Amount:{1}", _paymentId, _amount);
}

// 现代方式：字符串插值（C# 6.0+）
public override string ToString()
{
    return $"PaymentId:{_paymentId}, Amount:{_amount}";
}
```

编译器会将字符串插值转换为 `string.Format` 调用：

```csharp
$"PaymentId:{_paymentId}, Amount:{_amount}"
    
// 编译器转换为
string.Format("PaymentId:{0}, Amount:{1}", _paymentId, _amount)
```

// 注意，在记录日志的时候，请尽可能使用结构化日志，避免使用字符串拼接，这样可以优化性能





# 微软日志记录器的创建？

```csharp
// 如何理解下面的代码？
ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddDebug();
});
ILogger logger = loggerFactory.CreateLogger<Program>();
```

```csharp
// 首先是，LoggerFactory类有一个静态方法
public static ILoggerFactory Create(Action<ILoggingBuilder> configure)
{
    // 这个方法接受一个名为 configure 的参数
    // configure 的类型是 Action<ILoggingBuilder>
    // 这意味着 configure 是一个委托
    // 这个委托指向的函数必须：
    // 1. 接受一个 ILoggingBuilder 类型的参数
    // 2. 返回 void（Action 委托没有返回值）
}
```

```csharp
// 还原代码，不使用 Lambda 表达式长这样

private static void ConfigureLogging(ILoggingBuilder builder)
{
    builder.AddDebug();
}

static void Main()
{
    ILoggerFactory loggerFactory = LoggerFactory.Create(ConfigureLogging);
}
```

```csharp
// 第二步，ILoggingBuilder接口，有扩展方法
namespace Microsoft.Extensions.Logging
{
    public static class DebugLoggerFactoryExtensions
    {
        // 这是一个扩展方法
        // 关键字 "this" 表明这是一个扩展方法
        // 扩展的是 ILoggingBuilder 接口
        // 该扩展方法返回一个 ILoggingBuilder
		public static ILoggingBuilder AddDebug(this ILoggingBuilder builder)
    	{
            // 该方法内部又用了扩展方法
        	builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DebugLoggerProvider>());
        	return builder;
    	}
    }
}
```

```csharp
// 为什么需要扩展方法？
// 因为 ILoggingBuilder 接口本身很简单：
// 它只有一个 `Services` 属性！
// 所有的配置方法（`AddDebug`、`AddConsole`、`AddEventLog` 等）都是通过扩展方法添加的。
public interface
{
    IServiceCollection Services { get; }
}
```





# Serilog日志记录器的使用？

```csharp
// 1、ILogger 的配置和创建，非常的牛逼
// 非常好的运用了 Fluent API（流畅接口） 的设计模式

Serilog.ILogger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .CreateLogger();
```

```csharp
// 牛逼在哪？先来看看LoggerConfiguration类的构造函数

public LoggerSinkConfiguration WriteTo { get; internal set; }
public LoggerEnrichmentConfiguration Enrich { get; internal set; }

public LoggerConfiguration()
{
    // WriteTo 是LoggerConfiguration类中的一个LoggerSinkConfiguration类型的只读属性，负责输出配置
    WriteTo = new LoggerSinkConfiguration(this, delegate (ILogEventSink s)
    {
        _logEventSinks.Add(s);
    });
    
    // Enrich 是LoggerConfiguration类中的一个LoggerEnrichmentConfiguration类型的只读属性，负责丰富器配置
    Enrich = new LoggerEnrichmentConfiguration(this, delegate (ILogEventEnricher e)
    {
        _enrichers.Add(e);
    });
}
```

```csharp
// 再来看 LoggerSinkConfiguration 的扩展方法（只展示Console输出的）
// 完成了输出配置的同时，返回该LoggerConfiguration
// 然后可以继续调用WriteTo的扩展方法，实现多输出配置
// 其他属性同理

public static LoggerConfiguration Console(this LoggerSinkConfiguration sinkConfiguration, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", IFormatProvider? formatProvider = null, LoggingLevelSwitch? levelSwitch = null, LogEventLevel? standardErrorFromLevel = null, ConsoleTheme? theme = null, bool applyThemeToRedirectedOutput = false, object? syncRoot = null)
{
    if (sinkConfiguration == null)
    {
        throw new ArgumentNullException("sinkConfiguration");
    }

    if (outputTemplate == null)
    {
        throw new ArgumentNullException("outputTemplate");
    }

    ConsoleTheme theme2 = (((!applyThemeToRedirectedOutput && (System.Console.IsOutputRedirected || System.Console.IsErrorRedirected)) || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NO_COLOR"))) ? ConsoleTheme.None : (theme ?? SystemConsoleThemes.Literate));
    if (syncRoot == null)
    {
        syncRoot = DefaultSyncRoot;
    }

    OutputTemplateRenderer formatter = new OutputTemplateRenderer(theme2, outputTemplate, formatProvider);
    return sinkConfiguration.Sink(new ConsoleSink(theme2, formatter, standardErrorFromLevel, syncRoot), restrictedToMinimumLevel, levelSwitch);
}
```



