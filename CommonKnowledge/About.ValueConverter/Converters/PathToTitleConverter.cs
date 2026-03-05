using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace About.ValueConverter;

public sealed class PathToTitleConverter : BaseValueConverter
{
    public override object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture
    )
    {
        if (value is string path && File.Exists(path))
        {
            return Path.GetFileName(path);
        }
        // return DependencyProperty.UnsetValue; // 把这条 DP 的值重置为未赋值状态
        return Binding.DoNothing; // 这次更新就当什么都没发生”（既不改目标，也不改源，停止本次数据流）
    }
}
