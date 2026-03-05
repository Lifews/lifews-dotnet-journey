using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace About.Property;

class TextBoxHelper
{
    #region Title Attached Property   附加属性

    public static string GetTitle(DependencyObject obj)
    {
        return (string)obj.GetValue(TitleProperty);
    }

    public static void SetTitle(DependencyObject obj, string value)
    {
        obj.SetValue(TitleProperty, value);
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached(
        "Title",
        typeof(string),
        typeof(TextBoxHelper),
        new PropertyMetadata("")
    );

    #endregion

    #region 附加属性示例


    public static readonly DependencyPropertyKey HasTextPropertyKey =
        DependencyProperty.RegisterAttachedReadOnly(
            "HasText",
            typeof(bool),
            typeof(TextBoxHelper),
            new PropertyMetadata(false)
        );

    public static readonly DependencyProperty HasTextProperty =
        HasTextPropertyKey.DependencyProperty;

    public static bool GetHasText(DependencyObject obj)
    {
        return (bool)obj.GetValue(HasTextProperty);
    }

    //注意这里是对HasTextPropertyKey赋值
    public static void SetHasText(DependencyObject obj, bool value)
    {
        obj.SetValue(HasTextPropertyKey, value);
    }

    public static bool GetMonitorTextChange(DependencyObject obj)
    {
        return (bool)obj.GetValue(MonitorTextChangeProperty);
    }

    public static void SetMonitorTextChange(DependencyObject obj, bool value)
    {
        obj.SetValue(MonitorTextChangeProperty, value);
    }

    // Using a DependencyProperty as the backing store for MonitorTextChange.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MonitorTextChangeProperty =
        DependencyProperty.RegisterAttached(
            "MonitorTextChange",
            typeof(bool),
            typeof(TextBoxHelper),
            new PropertyMetadata(false, MintiorTextChangedPropertyChanged)
        );

    private static void MintiorTextChangedPropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not TextBox box)
        {
            throw new NotSupportedException();
        }
        if ((bool)e.NewValue)
        {
            box.TextChanged += TextChanged;
            SetHasText(box, !string.IsNullOrEmpty(box.Text));
        }
        else
        {
            box.TextChanged -= TextChanged;
            SetHasText(box, false);
        }
    }

    private static void TextChanged(object sender, TextChangedEventArgs e)
    {
        var box = (TextBox)sender;
        SetHasText(box, !string.IsNullOrEmpty(box.Text));
    }

    #endregion
}
