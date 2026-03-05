#if false

// 使用反射，完全不涉及特性

Type type = typeof(MyClass);

// 获取所有公共属性
foreach (var prop in type.GetProperties())
{
    Console.WriteLine($"Property: {prop.Name}");
}

// 动态创建实例并设置属性
object obj = Activator.CreateInstance(type);
type.GetProperty("Name").SetValue(obj, "Dynamic Object");

// 4. 动态调用方法
type.GetMethod("SayHello").Invoke(obj, null);

public class MyClass
{
    public string Name { get; set; }

    public void SayHello()
    {
        Console.WriteLine("Hello!");
    }
}

#endif
