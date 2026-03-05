using System.Windows;
using System.Windows.Controls;

namespace About.Event;

public class BubbleControl : Button
{
    // 1. 定义路由事件（冒泡事件）
    public static readonly RoutedEvent BubbleClickEvent = EventManager.RegisterRoutedEvent(
        "BubbleClick", // 事件名称
        RoutingStrategy.Bubble, // 路由策略：冒泡（WPF特有）
        typeof(RoutedEventHandler), // 事件处理程序类型
        typeof(BubbleControl) // 所有者类型（WPF特有）
    );

    // 2. 定义事件包装器（让外部可以用+=/-=订阅）
    public event RoutedEventHandler BubbleClick
    {
        add { AddHandler(BubbleClickEvent, value); }
        remove { RemoveHandler(BubbleClickEvent, value); }
    }

    // 3. 定义触发事件的方法
    protected virtual void OnBubbleClick()
    {
        // 创建事件参数（对应普通事件中的EventArgs）
        RoutedEventArgs args = new RoutedEventArgs(BubbleClickEvent, this);
        // 触发事件（对应普通事件中的 事件?.Invoke(this, args)）
        RaiseEvent(args);
    }

    // 在某个时机调用触发方法
    protected override void OnClick() // 比如在按钮点击时
    {
        base.OnClick(); // 先执行基类逻辑
        OnBubbleClick(); // 然后执行触发事件的方法
    }
}
