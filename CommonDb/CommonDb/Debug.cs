using CommonDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//初始化静态配置
var configuration = new ConfigurationBuilder().AddJsonFile("DbCfg/DbSettings.json").Build();
ConfigManager.SetConfiguration(configuration);
BaseDbContext.Init(ConfigManager.Configuration["ConnectionStrings:DefaultConnection"]);

//依赖注入
var builder = new ServiceCollection();
builder.AddScoped<BaseDbContext>();
builder.AddScoped<IHoleInfoService, HoleInfoService>();
var services = builder.BuildServiceProvider();

var holeService = services.GetRequiredService<IHoleInfoService>();

// 创建测试数据
HoleInfo hole01 = new HoleInfo()
{
    CreateTime = Convert.ToDateTime("2025-07-1"),
    pcbNo = 1,
    Index = 1,
    AreaIndex = 1,
    PointFCenterX = 300.000f,
    PointFCenterY = 300.21f,
};

List<HoleInfo> holeInfos = new List<HoleInfo>();

for (int i = 0; i < 100; i++)
{
    holeInfos.Add(
        new HoleInfo()
        {
            CreateTime = Convert.ToDateTime("2025-04-1"),
            pcbNo = i,
            Index = i,
            AreaIndex = i,
            PointFCenterX = i * 133.222f,
            PointFCenterY = i * 123.23f,
        }
    );
}

await holeService.AddListAsync(holeInfos);

//DateTime beginTime = Convert.ToDateTime("2025-06-01");
//DateTime endTime = Convert.ToDateTime("2025-09-01");
//var results1 = await holeService.QueryAsync(beginTime, endTime, 1, 20, item => item.pcbNo > 50);
//Console.WriteLine(
//    $"查询 {beginTime:yyyy-MM-dd} 到 {endTime:yyyy-MM-dd} 的记录数量: {results1.Count}"
//);
