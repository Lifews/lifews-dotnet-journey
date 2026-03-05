using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("settings.json")
    .Build();

var logLevel = configuration["Logging:LogLevel:System"];
Console.WriteLine($"LogLevel:{logLevel}");

//Json文件中没有"RandomValue"对象，会返回Null值
var randomValue = configuration["RandomValue"];
Console.WriteLine($"RandomValue:{randomValue}");