using Microsoft.Extensions.Configuration;

namespace CommonDb;


/// <summary>
/// 初始化静态配置：数据库连接字符串
/// </summary>
public static class ConfigManager
{
    public static IConfiguration? Configuration = null;

    public static void SetConfiguration(IConfiguration _configuration)
    {
        Configuration ??= _configuration;
    }

}