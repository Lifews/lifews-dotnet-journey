// 模仿实现序列化一个类

#if false

using System.ComponentModel;
using System.Reflection;

var stu = new Student
{
    Id = 1,
    Name = "张三",
    Age = 18,
    Class = "高三(1)班",
    Gender = Gender.Male,
};

Console.WriteLine(Serialize(stu));

string Serialize(object obj)
{
    var res = obj.GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(pi => pi.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false)
        // 过滤掉 [Browsable(false)] 标记的属性，这里第一次写会有疑惑，
        // GetCustomAttribute 可能返回 null ture false，当返回值不为false时，才保留
        // 这就是特性与反射结合使用的一个简单例子
        .Select(pi => new { key = pi.Name, value = pi.GetValue(obj) })
        .Select(kv => $"{kv.key}:{kv.value}");

    return string.Join(Environment.NewLine, res);
}

class Student
{
    public int Id { get; set; }

    // .NET Framework 自带有[Browsable] 特性，位于 System.ComponentModel
    [Browsable(false)]
    public string? Name { get; set; }

    public int Age { get; set; }

    public string? Class { get; set; }

    //可以用这种表达，给特性的构造函数中不出现的属性赋值
    [Browsable(true, Tag = "123")]
    public Gender Gender { get; set; }
}

enum Gender
{
    Male,
    Female,
}

// 也可以自定义特性
// 思考：AttributeUsage特性是谁在用？
// 是编译器在用
[AttributeUsage(AttributeTargets.Property)]
class BrowsableAttribute : Attribute
{
    public bool Browsable { get; set; }

    //假如有部分属性，构造函数中没有，该如何赋值呢？
    public string Tag { get; set; } = "123";

    public BrowsableAttribute(bool browsable)
    {
        Browsable = browsable;
    }
}

#endif
