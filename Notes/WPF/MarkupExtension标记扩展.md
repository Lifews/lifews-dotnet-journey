### 定义

`MarkupExtension` 是一个基类，用于在 XAML 中提供值的自定义方式。它允许你创建可重用的扩展，这些扩展可以在 XAML 中使用花括号 `{}` 语法调用。



### 核心特点

- **在 XAML 中直接使用**，无需代码后台
- **提供动态值**，而不是静态值
- **可接受参数**，支持配置
- **可重用**，一次定义，多处使用



### 内置的 MarkupExtension

WPF 提供了许多内置的 MarkupExtension：

- x:Static

```xaml
<!-- 引用静态属性、字段或枚举值 -->
<TextBlock Text="{x:Static local:MyClass.StaticProperty}"/>
<TextBlock Text="{x:Static system:DateTime.Now}"/>
```

- Binding

```xaml
<!-- 数据绑定 -->
<TextBox Text="{Binding Path=UserName, Mode=TwoWay}"/>
<TextBlock Text="{Binding ElementName=slider, Path=Value}"/>
```

- StaticResource / DynamicResource

```xaml
<!-- 资源引用 -->
<Button Background="{StaticResource MyBrush}"/>
<TextBlock Style="{DynamicResource MyTextStyle}"/>
```

- x:Type

```xaml
<!-- 获取类型 -->
<DataTemplate DataType="{x:Type local:Person}">
    <!-- 模板内容 -->
</DataTemplate>
```

- x:Array

```xaml
<!-- 创建数组 -->
<x:Array Type="sys:String">
    <sys:String>选项1</sys:String>
    <sys:String>选项2</sys:String>
</x:Array>
```



### 自定义 MarkupExtension

基本结构

```c#
public class MyExtension : MarkupExtension
{
    // 可以添加属性和字段
    public string MyProperty { get; set; }
    
    // 必须重写的方法
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        // 返回实际的值
        return "提供的值";
    }
}
```































