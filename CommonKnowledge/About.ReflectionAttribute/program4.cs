// 模仿实现Benchmark

#if false

using System.Diagnostics;
using System.Reflection;

BenchmarkRunner<simple>();

void BenchmarkRunner<T>(int count = 1_000_000)
    where T : new()
{
    var obj = new T();

    var res = typeof(T)
        .GetMethods()
        // 注意是   GetCustomAttribute
        // 而不是   GetCustomAttributes
        // 这个问题找了好久才发现……
        .Where(x => x.GetCustomAttribute<BenchmarkAttribute>() is not null)
        .ToList();

    Console.WriteLine(string.Join(Environment.NewLine, res));

    foreach (var method in res)
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < count; i++)
        {
            method.Invoke(obj, null);
        }
        Console.WriteLine(method.Name);
        Console.WriteLine(sw.ElapsedMilliseconds);
    }
}

public class simple
{
    private IEnumerable<int> list = Enumerable.Range(1, 10).ToArray();

    [Benchmark]
    public int CalcMinByLINQ()
    {
        return list.Min();
    }

    [Benchmark]
    public int CalcMinByNaive()
    {
        int min = int.MaxValue;
        foreach (int i in list)
        {
            if (i < min)
                min = i;
        }
        return min;
    }
}

[AttributeUsage(AttributeTargets.Method)]
class BenchmarkAttribute : Attribute { }

#endif

#if false

// 思考：如果在上述例子中，无法给BenchmarkRunner传泛型，又该怎么用反射呢？

using System.Diagnostics;
using System.Reflection;

BenchmarkRunner(typeof(Simple));

void BenchmarkRunner(Type type, int count = 1_000_000)
{
    // 通过这种方法动态创建实例
    var obj = Activator.CreateInstance(type);

    var res = type.GetMethods()
        // 注意是   GetCustomAttribute
        // 而不是   GetCustomAttributes
        // 这个问题找了好久才发现……
        .Where(x => x.GetCustomAttribute<BenchmarkAttribute>() is not null)
        .ToList();

    Console.WriteLine(string.Join(Environment.NewLine, res));

    foreach (var method in res)
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < count; i++)
        {
            method.Invoke(obj, null);
        }
        Console.WriteLine(method.Name);
        Console.WriteLine(sw.ElapsedMilliseconds);
    }
}

public class Simple
{
    private IEnumerable<int> list = Enumerable.Range(1, 10).ToArray();

    [Benchmark]
    public int CalcMinByLINQ()
    {
        return list.Min();
    }

    [Benchmark]
    public int CalcMinByNaive()
    {
        int min = int.MaxValue;
        foreach (int i in list)
        {
            if (i < min)
                min = i;
        }
        return min;
    }
}

[AttributeUsage(AttributeTargets.Method)]
class BenchmarkAttribute : Attribute { }

#endif
