using Microsoft.Extensions.DependencyInjection;

namespace MyLogging;

public static class MyLoggingExtensions
{
    // 扩展方法，这样写可以直接在Ioc中使用，而不需要在依赖注入时再去判断一下是单例还是瞬态
    public static void AddMyLogging(this IServiceCollection services)
    {
        services.AddSingleton<IMyLogger, MyLogger>();
    }
}
