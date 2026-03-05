#region 基础用法1

using System.Net;

List<int> list = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

var ressult0 = list.Where(n => n % 2 == 0).OrderBy(n => n);

foreach (var item in ressult0)
{
    Console.WriteLine(item);
}

#endregion


#region 基础用法2

int[] arr1 = [1, 2, 3, 4, 5];

int[] arr2 = [4, 5, 6, 7, 8];

var result1 = arr1.Intersect(arr2);

var result2 = arr1.Where(n => arr2.Contains(n));

foreach (var item in result2)
{
    Console.WriteLine(item);
}

#endregion


#region 基础用法3

var rnd = new Random(1234);

var arr = Enumerable.Range(1, 200).Select(_ => rnd.Next(20));

// 在 C# 的匿名对象初始化器里，如果你只写变量名不写属性名，编译器会自动用变量名作为属性名。
// 这叫 “投影初始化器”（projection initializer），是语法糖。
// 等价于 .Select(n => new { Key = n.Key, Count = n.Count() });
var result3 = arr.GroupBy(n => n).Select(n => new { n.Key, Count = n.Count() });

foreach (var group in result3)
{
    Console.WriteLine($"Number: {group.Key}, Count: {group.Count}");
}

#endregion


#region 展平

var mat = new int[][] { new[] { 1, 2, 3, 4 }, new[] { 5, 6, 7, 8 }, new[] { 9, 10, 11, 12 } };

var res = from row in mat from n in row select n;

foreach (var item in res.ToArray())
{
    Console.WriteLine(item);
}

// 等价于
var res1 = mat.SelectMany(x => x).ToArray();

foreach (var item in res.ToArray())
{
    Console.WriteLine(item);
}

#endregion


#region 笛卡尔积

var prods = Enumerable
    .Range(0, 5)
    .SelectMany(r => Enumerable.Range(0, 4), (l, r) => (l, r)) // 值类型元组
    .SelectMany(x => Enumerable.Range(0, 3), (l, r) => (l.l, l.r, r))
    .ToArray();

foreach (var item in prods)
{
    Console.WriteLine(item);
}

#endregion


#region 批量下载文件

var urls = new string[]
{
    "https://www.example.com/pic1.jpg",
    "https://www.example.com/pic2.jpg",
    "https://www.example.com/pic3.jpg",
};

var tasks = urls.Select(url => DownloadAsync(url, url.Split('/').Last())); // 异步下载

await Task.WhenAll(tasks);
Console.WriteLine("finished");

async Task DownloadAsync(string url, string filename)
{
    await Task.Delay(1000);
    Console.WriteLine($"{filename} download.");
}

#endregion
