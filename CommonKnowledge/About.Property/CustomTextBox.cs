using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace About.Property;

class CustomTextBox : TextBox
{
    #region IsHighlighted Dependency Property   依赖属性

    public bool IsHighlighted
    {
        get { return (bool)GetValue(IsHighlightedProperty); }
        set { SetValue(IsHighlightedProperty, value); }
    }

    public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
        "IsHighlighted",
        typeof(bool),
        typeof(CustomTextBox),
        new PropertyMetadata(false)
    );

    #endregion


    #region HasText1 Dependency Property   只读依赖属性

    public static readonly DependencyPropertyKey HasText1PropertyKey;
    public static readonly DependencyProperty HasText1Property;

    /// <summary>
    /// 构造函数
    /// </summary>
    static CustomTextBox()
    {
        HasText1PropertyKey = DependencyProperty.RegisterReadOnly(
            "HasText1",
            typeof(bool),
            typeof(CustomTextBox),
            new PropertyMetadata(false)
        );
        HasText1Property = HasText1PropertyKey.DependencyProperty;
    }

    #endregion
}
