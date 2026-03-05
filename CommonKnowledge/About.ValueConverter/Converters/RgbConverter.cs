using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace About.ValueConverter;

public class RgbConverter : IMultiValueConverter
{
    // 3 滑块 → Color
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 3)
            return null;
        if (!IsByte(values[0]) || !IsByte(values[1]) || !IsByte(values[2]))
            return null;

        byte r = (byte)(double)values[0];
        byte g = (byte)(double)values[1];
        byte b = (byte)(double)values[2];

        return new SolidColorBrush(Color.FromRgb(r, g, b));
    }

    // Color → 3 滑块
    public object[] ConvertBack(
        object value,
        Type[] targetTypes,
        object parameter,
        CultureInfo culture
    )
    {
        if (value is SolidColorBrush brush)
        {
            var c = brush.Color;
            return new object[] { (double)c.R, (double)c.G, (double)c.B };
        }
        // 无法拆分就原路返回，告诉绑定“别动”
        return new object[] { Binding.DoNothing, Binding.DoNothing, Binding.DoNothing };
    }

    private static bool IsByte(object v) => v is double d && d >= 0 && d <= 255;
}
