using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace About.Event;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // 为各个元素添加事件处理程序（演示事件冒泡）

        // 1.处理BubbleClick事件
        InnerBorder.AddHandler(
            BubbleControl.BubbleClickEvent,
            new RoutedEventHandler(HandleBubbleClick),
            true
        ); // handledEventsToo设置为true，即使事件已被处理也会触发

        // 2. 中间StackPanel处理BubbleClick事件
        MiddlePanel.AddHandler(
            BubbleControl.BubbleClickEvent,
            new RoutedEventHandler(HandleBubbleClick)
        );

        // 3. 外层Border处理BubbleClick事件
        OuterBorder.AddHandler(
            BubbleControl.BubbleClickEvent,
            new RoutedEventHandler(HandleBubbleClick)
        );

        // 4. 主StackPanel处理BubbleClick事件
        MainPanel.AddHandler(
            BubbleControl.BubbleClickEvent,
            new RoutedEventHandler(HandleBubbleClick)
        );

        // 5. 窗口本身处理BubbleClick事件
        this.AddHandler(BubbleControl.BubbleClickEvent, new RoutedEventHandler(HandleBubbleClick));

        // 6. 为自定义控件本身添加事件处理程序
        MyBubbleControl.BubbleClick += MyBubbleControl_BubbleClick;
    }

    private void HandleBubbleClick(object sender, RoutedEventArgs e)
    {
        string senderName = ((FrameworkElement)sender).Name;
        string sourceName = ((FrameworkElement)e.Source).Name;

        LogEvent($"处理者: {senderName}, 事件源: {sourceName}, 事件类型: {e.RoutedEvent.Name}");

        // 演示如何标记事件为已处理
        if (senderName == "MiddlePanel")
        {
            e.Handled = true;
            LogEvent("*** MiddlePanel将事件标记为已处理，事件将停止向上冒泡 ***");
        }
    }

    private void MyBubbleControl_BubbleClick(object sender, RoutedEventArgs e)
    {
        LogEvent("BubbleControl处理定义的事件");
    }

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        LogEvent("普通按钮Click事件（非路由事件冒泡）");
    }

    private void LogEvent(string message)
    {
        EventLog.Text += $"\n{message}";
    }

    private void ClearLog_Click(object sender, RoutedEventArgs e)
    {
        EventLog.Text = "事件日志：";
    }

    // 演示隧道事件（Preview事件）
    protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    {
        // 摘要:
        //     当未处理的 System.Windows.Input.Mouse.PreviewMouseDown 附加路由事件在路由路径中到达继承自本类的元素时触发。
        //     实现此方法可为该事件添加类处理功能。
        //
        // 参数:
        //   e:
        //     包含事件数据的 System.Windows.Input.MouseButtonEventArgs 对象。该事件数据报告一个或多个鼠标按钮被按下。
        base.OnPreviewMouseDown(e);
        if (e.Source is FrameworkElement element)
        {
            LogEvent($"隧道事件: PreviewMouseDown - 源: {element.Name}");
        }
    }
}
