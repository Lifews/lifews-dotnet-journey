## 语言集成查询LINQ



### 基本概念

LINQ（Language Integrated Query），C# 的革命性特性，它将查询能力直接集成到编程语言中。

### 常见用途

- .NET原生集合（List, Array, Dictionary, etc.）
- SQL数据库（尤其配合ORM）
- XML文档
- JSON文档（Newtonsoft.Json）

### 常见功能

- 排序、筛选、选择
- 分组、聚合、合并
- 最大值、最小值、求和、求平均、求数量
- ……

### 两种形式

- 查询表达式 query expression

```C#
var result = from p in products
             where p.Price > 100
             orderby p.Name descending
             select p.Name;
```

```C#
var query = from c in customers
            join o in orders on c.Id equals o.CustomerId
            where o.Date.Year == 2023
            select new { c.Name, o.Total };
```

- 链式表达式 chained expression

```
var result = products
             .Where(p => p.Price > 100)
             .OrderByDescending(p => p.Name)
             .Select(p => p.Name);
```

```
var query = customers
            .Join(orders,
                  c => c.Id,
                  o => o.CustomerId,
                  (c, o) => new { c, o })
            .Where(x => x.o.Date.Year == 2023)
            .Select(x => new { x.c.Name, x.o.Total });
```



### 重要概念

- 延迟执行 defer
- 消耗 exhaust ---- 常见的消耗方式：
  - 遍历 foreach
  - ToList()、ToArray()、ToDictionary()
  - Count()、Min()、Max()、Sum()
  - Take()、First()、Last()
- Linq不仅仅是可枚举类型的扩展方法
  - IEnumerable
  - IOrderedEnumerable
  - IQueryable
  - ParallelQuery(并行查询)





























