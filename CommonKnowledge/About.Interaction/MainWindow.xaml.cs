using System.Diagnostics.Metrics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Xaml.Behaviors;

namespace AllAboutInteraction;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowViewModel();
    }
}

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _text;

    public AsyncRelayCommand LoadedCommand { get; }

    public MainWindowViewModel()
    {
        LoadedCommand = new(Loaded);
    }

    private async Task Loaded()
    {
        await Task.Delay(1000);
        Text = "Hello World!";
    }
}

class BorderBehavior : Behavior<Border>
{
    protected override void OnAttached()
    {
        // AssociatedObject 是 Behavior（或 Trigger）内部的一个 protected 属性，指向 “被你附加这个动作的那个控件”。
        //“我这条 Behavior 贴在谁身上，AssociatedObject 就是谁。”这里就是Border
        AssociatedObject.Background = Brushes.Blue;
        // lambda，参数用下划线丢弃了。
        AssociatedObject.MouseEnter += (_, _) => AssociatedObject.Background = Brushes.Orange;
    }

    protected override void OnDetaching() { }
}

class ClearTextBehavior : Behavior<Button>
{
    public TextBox Target
    {
        get { return (TextBox)GetValue(TargetProperty); }
        set { SetValue(TargetProperty, value); }
    }

    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
        "Target",
        typeof(TextBox),
        typeof(ClearTextBehavior),
        new PropertyMetadata(null)
    );

    // Behavior 被实例化并正式附加到 AssociatedObject 之后立即调用
    protected override void OnAttached()
    {
        AssociatedObject.Click += ButtonClick;
    }

    // Behavior 从 AssociatedObject 上 移除或元素被卸载时调用
    protected override void OnDetaching()
    {
        AssociatedObject.Click -= ButtonClick;
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
        Target?.Clear();
    }
}

class MouseWheelBehavior : Behavior<TextBox>
{
    public int MaxValue { get; set; }
    public int MinValue { get; set; }
    public int Scale { get; set; } = 1;

    protected override void OnAttached()
    {
        AssociatedObject.MouseWheel += MouseWheel;
    }

    private void MouseWheel(object sender, MouseWheelEventArgs e)
    {
        try
        {
            var value = int.Parse(AssociatedObject.Text);
            if (e.Delta > 0)
            {
                value += Scale;
            }
            else if (e.Delta < 0)
            {
                value -= Scale;
            }
            if (value > MaxValue)
                value = MaxValue;
            if (value < MinValue)
                value = MinValue;
            AssociatedObject.Text = value.ToString();
        }
        catch
        {
            return;
        }
    }
}
