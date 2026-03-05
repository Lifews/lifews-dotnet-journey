#if false

// 延迟执行（deferred execution）是 LINQ 的默认行为

// 定义时不跑，第一次要数据时才跑，而且每次要数据都可能重新跑一遍。

// 避免重复消耗可以：
// 1. 使用 ToList()、ToArray() 等方法将查询结果缓存到内存中。
// 2. 使用变量存储查询结果，避免重复执行查询。

var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var query = list.Select(n =>
{
    Thread.Sleep(1000); // 模拟耗时操作
    return n * n;
});

Console.WriteLine("执行到这里的时候没有延时");

foreach (var item in query)
{
    Console.WriteLine(item);
}

#endif