using System.Windows.Markup;

namespace Demo.ComboBoxBinding;

//在 XAML 中通过标记扩展语法直接获取枚举的所有值，作为 ComboBox 等控件的数据源。
class EnumBindingSourceExtension : MarkupExtension
{
    private Type? _enumType;

    public Type? EnumType
    {
        get => _enumType;
        set
        {
            if (value != _enumType)
            {
                if (value != null)
                {
                    Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                        throw new ArgumentException("Type must be for an Enum.");
                }

                _enumType = value;
            }
        }
    }

    public EnumBindingSourceExtension() { }

    public EnumBindingSourceExtension(Type enumType)
    {
        EnumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (_enumType == null)
            throw new InvalidOperationException("The EnumType must be specified.");

        var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
        var enumValues = Enum.GetValues(actualEnumType);

        //如果原始类型就是枚举类型（非可空），直接返回枚举值
        if (actualEnumType == _enumType)
            return enumValues;

        //如果是可空枚举类型（Nullable<MyEnum>），创建一个长度+1的数组
        //从索引1开始复制枚举值，这样索引0的位置就是 null
        var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }
}
