using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace About.ValueConverter;

// 重写一下bool to visibility，多加一些属性
public class BoolToVisibilityConverter : IValueConverter
{
    // 创建一个ValueConverter的单例
    public static BoolToVisibilityConverter Instance { get; } = new BoolToVisibilityConverter();
    public bool UseHidden { get; set; }
    public bool IsReversed { get; set; }

    public object Convert(
        object value, // 绑定的源属性当时实际值。比如 Text="{Binding Age, Converter={x:MyConv}}"，这里 value 就是 Age 的 int 值。
        Type targetType, // 目标依赖属性需要的类型。上面例子里 TextBox.Text 是 string，因此 targetType == typeof(string)。如果是 Background 绑定，那 targetType == typeof(Brush)。你必须返回该类型（或 null/DependencyProperty.UnsetValue），否则框架会抛 InvalidCastException。
        object parameter, // 允许在 XAML 里传额外信息
        CultureInfo culture // 线程当前 CultureInfo，做 ToString(..., culture) 或 DateTime.Parse 时必须用它，才能尊重用户区域设置。
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

    // （目标 → 源，双向才用）
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
