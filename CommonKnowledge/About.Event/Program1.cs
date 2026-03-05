// ============== 基础的事件发布-订阅模式示例 ==============

#if false

// 事件参数类 
public class TemperatureChangedEventArgs : EventArgs
{
    public string SensorId { get; }
    public double OldTemperature { get; }
    public double NewTemperature { get; }
    public DateTime Timestamp { get; }

    public TemperatureChangedEventArgs(string sensorId, double oldTemp, double newTemp)
    {
        SensorId = sensorId;
        OldTemperature = oldTemp;
        NewTemperature = newTemp;
        Timestamp = DateTime.Now;
    }
}

public class TemperatureCriticalEventArgs : EventArgs
{
    public string SensorId { get; }
    public double Temperature { get; }
    public string WarningLevel { get; } // "HIGH" or "LOW"

    public TemperatureCriticalEventArgs(string sensorId, double temp, string level)
    {
        SensorId = sensorId;
        Temperature = temp;
        WarningLevel = level;
    }
}

// 发布者类 
public class TemperatureSensor
{
    private string _id;
    private double _currentTemperature;
    private readonly Random _random = new Random();
    private Timer _timer;

    // 定义事件 - 温度变化事件
    public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;

    // 定义事件 - 温度临界事件
    public event EventHandler<TemperatureCriticalEventArgs> TemperatureCritical;

    public string Id => _id;
    public double CurrentTemperature => _currentTemperature;

    public TemperatureSensor(string id, double initialTemp = 20.0)
    {
        _id = id;
        _currentTemperature = initialTemp;

        // 模拟温度变化
        _timer = new Timer(SimulateTemperatureChange, null, 1000, 2000);
    }

    private void SimulateTemperatureChange(object state)
    {
        double oldTemp = _currentTemperature;

        // 随机温度变化 -2°C 到 +3°C
        double change = (_random.NextDouble() * 5) - 2;
        _currentTemperature += change;

        // 限制温度范围
        _currentTemperature = Math.Max(-10, Math.Min(50, _currentTemperature));

        // 触发温度变化事件
        OnTemperatureChanged(oldTemp, _currentTemperature);

        // 检查临界值
        CheckCriticalTemperature(_currentTemperature);
    }

    protected virtual void OnTemperatureChanged(double oldTemp, double newTemp)
    {
        // 线程安全的触发方式
        TemperatureChanged?.Invoke(this, new TemperatureChangedEventArgs(_id, oldTemp, newTemp));
    }

    protected virtual void OnTemperatureCritical(double temp, string level)
    {
        TemperatureCritical?.Invoke(this, new TemperatureCriticalEventArgs(_id, temp, level));
    }

    private void CheckCriticalTemperature(double temp)
    {
        if (temp > 40)
        {
            OnTemperatureCritical(temp, "HIGH");
        }
        else if (temp < 0)
        {
            OnTemperatureCritical(temp, "LOW");
        }
    }

    public void StopMonitoring()
    {
        _timer?.Dispose();
        Console.WriteLine($"[{_id}] 传感器已停止监控");
    }
}

// 订阅者类 
public class TemperatureDisplay
{
    private string _displayName;

    public TemperatureDisplay(string name)
    {
        _displayName = name;
        Console.WriteLine($"[{_displayName}] 温度显示器已创建");
    }

    // 明确的订阅方法 - 显示订阅意图
    public void SubscribeToSensor(TemperatureSensor sensor)
    {
        Console.WriteLine($"[{_displayName}] 开始订阅传感器: {sensor.Id}");

        // 订阅温度变化事件
        sensor.TemperatureChanged += OnTemperatureChanged;

        // 订阅温度临界事件
        sensor.TemperatureCritical += OnTemperatureCritical;

        Console.WriteLine($"[{_displayName}] 已订阅传感器: {sensor.Id}");
    }

    // 明确的取消订阅方法
    public void UnsubscribeFromSensor(TemperatureSensor sensor)
    {
        Console.WriteLine($"[{_displayName}] 取消订阅传感器: {sensor.Id}");

        sensor.TemperatureChanged -= OnTemperatureChanged;
        sensor.TemperatureCritical -= OnTemperatureCritical;

        Console.WriteLine($"[{_displayName}] 已取消订阅传感器: {sensor.Id}");
    }

    // 事件处理方法
    private void OnTemperatureChanged(object sender, TemperatureChangedEventArgs e)
    {
        Console.WriteLine($"[{_displayName}] 温度变化 - 传感器: {e.SensorId}");
        Console.WriteLine($"  时间: {e.Timestamp:HH:mm:ss}");
        Console.WriteLine($"  旧温度: {e.OldTemperature:F1}°C");
        Console.WriteLine($"  新温度: {e.NewTemperature:F1}°C");
        Console.WriteLine($"  变化: {(e.NewTemperature - e.OldTemperature):+0.0;-0.0}°C");
        Console.WriteLine();
    }

    private void OnTemperatureCritical(object sender, TemperatureCriticalEventArgs e)
    {
        string warningType = e.WarningLevel  "HIGH" ? "高温" : "低温";
        Console.ForegroundColor = e.WarningLevel  "HIGH" ? ConsoleColor.Red : ConsoleColor.Cyan;
        Console.WriteLine($"[{_displayName}] ⚠️  {warningType}警告 - 传感器: {e.SensorId}");
        Console.WriteLine($"  当前温度: {e.Temperature:F1}°C");
        Console.ResetColor();
        Console.WriteLine();
    }
}

public class TemperatureLogger
{
    private List<string> _logEntries = new List<string>();

    public void SubscribeToSensor(TemperatureSensor sensor)
    {
        Console.WriteLine($"[数据记录器] 开始记录传感器: {sensor.Id}");
        sensor.TemperatureChanged += LogTemperatureChange;
    }

    private void LogTemperatureChange(object sender, TemperatureChangedEventArgs e)
    {
        string logEntry =
            $"{e.Timestamp:yyyy-MM-dd HH:mm:ss} | {e.SensorId} | "
            + $"{e.OldTemperature:F1}°C → {e.NewTemperature:F1}°C";
        _logEntries.Add(logEntry);

        // 每5条记录显示一次
        if (_logEntries.Count % 5  0)
        {
            Console.WriteLine("= 温度记录摘要 =");
            for (int i = Math.Max(0, _logEntries.Count - 5); i < _logEntries.Count; i++)
            {
                Console.WriteLine($"  {_logEntries[i]}");
            }
            Console.WriteLine();
        }
    }

    public void ShowAllLogs()
    {
        Console.WriteLine("= 完整温度记录 =");
        foreach (var log in _logEntries)
        {
            Console.WriteLine(log);
        }
        Console.WriteLine($"总计: {_logEntries.Count} 条记录");
    }
}

// 示例主程序 
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("= 温度监控系统启动 =\n");

        // 1. 创建发布者（温度传感器）
        var kitchenSensor = new TemperatureSensor("厨房传感器");
        var bedroomSensor = new TemperatureSensor("卧室传感器", 22.5);

        // 2. 创建订阅者（显示器）
        var mainDisplay = new TemperatureDisplay("主显示屏");
        var mobileDisplay = new TemperatureDisplay("移动设备");
        var logger = new TemperatureLogger();

        // 3. 订阅者订阅传感器事件
        mainDisplay.SubscribeToSensor(kitchenSensor);
        mainDisplay.SubscribeToSensor(bedroomSensor);

        mobileDisplay.SubscribeToSensor(kitchenSensor);

        logger.SubscribeToSensor(kitchenSensor);
        logger.SubscribeToSensor(bedroomSensor);

        Console.WriteLine("\n= 监控进行中（10秒）=\n");

        // 4. 让监控运行一段时间
        await Task.Delay(10000);

        // 5. 演示取消订阅
        Console.WriteLine("\n= 演示取消订阅 =\n");
        mobileDisplay.UnsubscribeFromSensor(kitchenSensor);

        Console.WriteLine("\n= 继续监控（5秒）=\n");
        await Task.Delay(5000);

        // 6. 停止传感器
        kitchenSensor.StopMonitoring();
        bedroomSensor.StopMonitoring();

        // 7. 显示记录
        logger.ShowAllLogs();

        Console.WriteLine("\n= 系统关闭 =");
        Console.ReadKey();
    }
}

#endif
