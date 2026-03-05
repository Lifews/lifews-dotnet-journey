// 扩展方法的基本用法

using System.Reflection;
using System.Xml.Linq;

var user = new User
{
    Id = 1,
    Name = "Vincent",
    Email = "tsuki_tsuki1993@163.com",
    IsActive = true,
};

// 如果不加this，则要这样调用
UserExtensions.IsValid(user);

// 如果加上this，则可以这样调用
user.IsValid();

// 思考：如何通过反射获得扩展方法？
var method = typeof(UserExtensions).GetMethod("Intro", BindingFlags.Static | BindingFlags.Public);

//public object? Invoke(object? obj, object?[]? parameters);
//这是 MethodInfo.Invoke() 方法的签名，用于通过反射调用方法。
//obj: 要调用方法的对象实例
//对于实例方法：传递方法所属的对象实例
//对于静态方法：传递 null
//parameters: 方法参数的数组
//对于无参数的方法：传递 null 或空数组
method?.Invoke(null, new object[] { user });

// 1.定义静态类
static class UserExtensions
{
    // 2. 静态方法 + this关键字
    public static bool IsValid(this User user)
    // this 加在第一个参数前，关键字this表示该方法是扩展方法，如果没有this，则表示普通的静态方法。
    {
        return !string.IsNullOrEmpty(user.Name)
            && !string.IsNullOrEmpty(user.Email)
            && user.Email.Contains('@')
            && user.IsActive;
    }

    public static void Intro(this User user)
    {
        Console.WriteLine(
            $"Id: {user.Id}, Name: {user.Name}, Email: {user.Email}, IsActive: {user.IsActive}"
        );
    }
}

class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}
