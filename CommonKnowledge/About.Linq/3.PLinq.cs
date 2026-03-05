#if false

var arr = Enumerable
    .Range(1, 10)
    .ToArray()
    .AsParallel() // 并行查询,会返回一个ParallelQuery
    .AsOrdered()
    .Select(x =>
    {
        Thread.Sleep(500);
        return x * x;
    })
    .AsSequential( );// 转化回IEnumerable

foreach (var x in arr)
{
    Console.WriteLine(x);
}

#endif
