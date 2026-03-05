using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Demo.ComboBoxBinding;

//用于在 WPF 数据绑定中将枚举值转换为其 DescriptionAttribute 中定义的描述文本。
public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return DependencyProperty.UnsetValue; //DependencyProperty.UnsetValue 是 WPF 的特殊值，表示"未设置值",绑定引擎会忽略这个值，不会显示错误

        return GetEnumDescription(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }

    private string GetEnumDescription(object enumObj)
    {
        var fi = enumObj.GetType().GetField(enumObj.ToString());//通过反射获取该枚举值对应的字段信息

        DescriptionAttribute[] attributes = (DescriptionAttribute[])
            fi.GetCustomAttributes(typeof(DescriptionAttribute), false);//获取该字段上定义的所有 DescriptionAttribute 特性

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return enumObj.ToString();
    }
}
