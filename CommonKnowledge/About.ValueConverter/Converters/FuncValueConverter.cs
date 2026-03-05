using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace About.ValueConverter;

/// <summary>
/// 通用的函数式值转换器<br/>
/// 通过委托（Func）来动态定义转换逻辑，而不是为每种转换场景创建单独的转换器类。<br/>
/// 适用于单次使用的转换器，可以不用单独创一个转换器类，而是把转换逻辑写在ViewModel中。
/// </summary>
/// <typeparam name="TIn"></typeparam>
/// <typeparam name="TOut"></typeparam>
public sealed class FuncValueConverter<TIn, TOut> : IValueConverter
{
    private readonly Func<TIn, TOut> _converter;
    private readonly Func<TOut, TIn> _converterBack;

    public FuncValueConverter(Func<TIn, TOut> converter, Func<TOut, TIn>? converterBack = null)
    {
        _converter = converter;
        _converterBack = converterBack;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var typeConverter = TypeDescriptor.GetConverter(typeof(TIn));

        if (value == null)
            return DependencyProperty.UnsetValue;

        if (value is not TIn obj)
        {
            if (typeConverter.CanConvertFrom(value.GetType()))
            {
                obj = (TIn)typeConverter.ConvertFrom(value);
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
        return _converter(obj);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
