using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace About.ValueConverter;

public class BoolToVisibilityMarkupExtension : BaseValueConverter
{
    public static BoolToVisibilityConverter Instance { get; } = new BoolToVisibilityConverter();
    public bool UseHidden { get; set; } = false;
    public bool IsReversed { get; set; } = false;

    public override object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture
    )
    {
        if (value is bool boolValue)
        {
            if (IsReversed)
                boolValue = !boolValue;

            if (boolValue)
                return Visibility.Visible;
            return UseHidden ? Visibility.Hidden : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }
}

// 可以尝试声明一个Converter的基类，减少重复性代码
public abstract class BaseValueConverter : MarkupExtension, IValueConverter
{
    public abstract object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture
    );

    public virtual object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture
    )
    {
        return Binding.DoNothing;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        // MarkupExtension 的作用：MarkupExtension 用于在 XAML 中提供值，当 XAML 解析器遇到 {x:Type ...}、{Binding ...} 等标记扩展时，会调用 ProvideValue 方法。
        // return this：表示这个 MarkupExtension 实例本身将作为转换器对象提供给绑定使用。
        return this;
    }
}
