# 委托Delegate

委托有什么用？

1. 将函数作为函数的参数进行传递
2. 声明事件并用来注册

强委托类型

```c#
Action<T1>
Func<T1,TResult>
```



# 多播委托Multicast Delegate

在C#中，能创建到的所有委托都是多播委托，除非你专门去创建一个普通的委托。

它可以引用**多个方法**，当委托被调用时，所有关联的方法都会按顺序执行。这是实现**观察者模式**和**事件驱动编程**的核心机制。

- 调用委托时，如果其中的一个委托报错，则后面的不会被调用
- 只有最后一个的返回值才会作为委托的返回值
- 其内部用一个`object? _invocationList`的数组存储，因为是数组，所以remove的复杂度是*O（n）*
- 线程不安全

## 委托为什么不等于函数指针？

- 委托可以“指向”多个函数，但函数指针做不到
- 委托可以“指向”同一个函数多次
- C#中，函数是包含在类中的，当方法被添加到委托中时，这个方法所在的类的实例，也被作为信息传递了过去；而C/C++的函数指针只是函数的入口地址,并不具备这些信息

```C#
// 定义委托
public delegate void NotificationHandler(string message);

class Program
{
    static void Main()
    {
        // 创建多播委托实例
        NotificationHandler notify = SendEmail;
        
        // 添加更多方法
        notify += SendSMS;
        notify += LogMessage;
        
        // 调用多播委托 - 所有方法按添加顺序执行
        notify("系统更新完成！");
        
        // 移除方法
        notify -= SendSMS;
        notify("SMS通知已移除");
    }

    static void SendEmail(string msg) 
        => Console.WriteLine($"邮件发送: {msg}");

    static void SendSMS(string msg) 
        => Console.WriteLine($"短信发送: {msg}");

    static void LogMessage(string msg) 
        => Console.WriteLine($"日志记录: {msg} - {DateTime.Now}");
}
```



# 事件Event

希望一个类的某些成员在发生变化时能够被外界观测到

- `CollectionChanged`
- `TextChanged`

标准.NET事件模式

```C#
delegate EventHander(object sender, EventArgs e)
EventArgs
Button.Click
TextBox.TextChanged
```

## 事件的本质，是C#提供的语法糖

1. 讲委托以私有变量的形式封装在类内，不让外面访问
2. 对于委托进行了封装，定义add与remove方法
3. 在add与remove中通过互锁的方式提供了线程安全性，线程锁

```C#
// 使用事件而不是公开委托字段，可提供更好的封装和安全性
public class Publisher
{
    // event：声明这是一个事件
	// EventHandler<EventArgs>：事件的委托类型（处理程序签名）
	// ImportantEvent：事件名称
    public event EventHandler<EventArgs> ImportantEvent
    
    protected virtual void OnImportantEvent()
    {
        ImportantEvent?.Invoke(this, EventArgs.Empty);
    }
}
```

## 事件基本用法总结

```C#
// 发布者

public class Publisher
{
    // ✅ 1、定义事件
    public event EventHandler SomethingHappened;
    // 或者使用自定义委托
    public event Action<object, string> CustomEvent;
    
    // ⚠️ 2、定义如何触发事件：
    // 通常有两种方式：
    
    // 方式1：提供触发事件的方法（推荐）
    public void DoSomething()
    {
        // ...执行某些操作...
        OnSomethingHappened(); // 触发事件
    }
    
    // 方式2：直接提供触发方法（保护方法，遵循.NET设计模式）
    protected virtual void OnSomethingHappened()
    {
        // 触发事件的标准方式
        SomethingHappened?.Invoke(this, EventArgs.Empty);
    }
}
```

```C#
// 订阅者

public class Subscriber
{
    // ✅ 1、有事件处理方法
    private void HandleEvent(object sender, EventArgs e)
    {
        Console.WriteLine("事件发生了！");
    }
    
    // ✅ 2、有订阅事件的方法：通常需要传入发布者对象
    public void SubscribeToPublisher(Publisher publisher)
    {
        // 订阅事件
        publisher.SomethingHappened += HandleEvent;
        // 或者使用Lambda表达式：
        // publisher.SomethingHappened += (s, e) => { ... };
    }
    
    // ✅ 3、有取消订阅事件的方法
    public void UnsubscribeFromPublisher(Publisher publisher)
    {
        publisher.SomethingHappened -= HandleEvent;
    }
}
```



# 路由事件 RoutedEven

任何继承自`UIElement`或`ContentElement`的类都可以定义和使用路由事件。WPF中的路由事件主要用在UI元素上。

## 路由事件基本用法总结

```C#
// 发布者

public class BubbleControl : Button
{
    // 1. 定义路由事件
    public static readonly RoutedEvent BubbleClickEvent = 
        EventManager.RegisterRoutedEvent(...);
    
    // 2. 事件包装器（让外部可以用+=/-=）
    public event RoutedEventHandler BubbleClick
    {
        add { AddHandler(BubbleClickEvent, value); }
        remove { RemoveHandler(BubbleClickEvent, value); }
    }
    
    // 3. 触发事件的方法
    protected virtual void OnBubbleClick()
    {
        RoutedEventArgs args = new RoutedEventArgs(BubbleClickEvent, this);
        RaiseEvent(args);
    }
    
    // 4. 在某个时机触发（比如点击时）
    protected override void OnClick()
    {
        base.OnClick();
        OnBubbleClick();
    }
}
```

```C#
// 订阅者


public class MyWindow : Window
{
    // 5. 事件处理方法
    private void HandleBubbleClick(object sender, RoutedEventArgs e)
    {
        Console.WriteLine($"路由事件处理，源：{e.Source}");
    }
    
    public MyWindow()
    {
        var control = new BubbleControl();
        
        // 6. 订阅事件（需要传入控件实例）
        control.BubbleClick += HandleBubbleClick;
        
        // 或者用AddHandler
        // control.AddHandler(BubbleControl.BubbleClickEvent, 
        //     new RoutedEventHandler(HandleBubbleClick));
        
        // 7. 也可以订阅到父容器
        this.AddHandler(BubbleControl.BubbleClickEvent,
            new RoutedEventHandler(HandleBubbleClick));
    }
}
```

如果没写包装器

```C#
// 2. 事件包装器（让外部可以用+=/-=）
public event RoutedEventHandler BubbleClick
{
    add { AddHandler(BubbleClickEvent, value); }
    remove { RemoveHandler(BubbleClickEvent, value); }
}
```

则不能在外部使用类似这样的语法订阅事件

```C#
control.BubbleClick += HandleBubbleClick;
```

只能用这样的语法去写，其中这里的 new RoutedEventHandler 是一个委托，HandleBubbleClick是与委托签名相同的方法

```C#
control.AddHandler(BubbleControl.BubbleClickEvent, 
    new RoutedEventHandler(HandleBubbleClick));
```



# .NET和WPF中的一些事件命名约定



## 1. 事件

基本规则：

- 使用**动词或动词短语**，表示发生的行为
- 使用**现在时**表示即将发生，**过去时**表示已经发生
- 遵循**PascalCase**命名法

常见模式：

```C#
// ✅ 正确示例：
public event EventHandler Clicked;           // 已完成
public event EventHandler Clicking;          // 进行中
public event EventHandler TextChanged;       // 已完成
public event EventHandler ValueChanging;     // 进行中
public event EventHandler SelectionChanged;  // 已完成
public event EventHandler MouseDown;         // 进行中
public event EventHandler MouseEnter;        // 进行中
public event EventHandler Loaded;            // 已完成
public event EventHandler Closing;           // 进行中
public event EventHandler Closed;            // 已完成

// ❌ 避免的命名：
public event EventHandler click;             // 应使用PascalCase
public event EventHandler OnClick;           // 不要以"On"开头（这是方法名）
public event EventHandler ClickEvent;        // 避免"Event"后缀
```

特殊情况：

```C#
// Preview事件（隧道事件）：
public event EventHandler PreviewMouseDown;  // 隧道事件以"Preview"开头
public event EventHandler PreviewKeyDown;

// 附加事件（Attached Events）：
public static readonly RoutedEvent GotFocusEvent;  // 路由事件以"Event"后缀
public static readonly RoutedEvent LostFocusEvent;
```



## 2. 触发事件的函数

标准模式：`On + EventName`

```C#
// ✅ 正确示例：
protected virtual void OnClick(EventArgs e)
{
    Click?.Invoke(this, e);
}

protected virtual void OnTextChanged(TextChangedEventArgs e)
{
    TextChanged?.Invoke(this, e);
}

protected virtual void OnPropertyChanged(string propertyName)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

// WPF路由事件：
protected virtual void OnBubbleClick()
{
    RaiseEvent(new RoutedEventArgs(BubbleClickEvent, this));
}

// ❌ 避免的命名：
protected virtual void Click()              // 缺少"On"前缀
protected virtual void RaiseClick()         // 不使用"Raise"前缀
protected virtual void FireClick()          // 不使用"Fire"
```



## 3. 订阅事件的函数

通常不需要专门的方法，但如果需要：

```C#
// ✅ 正确示例：
public void SubscribeToEvents()
{
    _publisher.Event1 += Handler1;
    _publisher.Event2 += Handler2;
}

public void AddEventHandlers()
{
    _button.Click += OnButtonClick;
    _textBox.TextChanged += OnTextChanged;
}

// 更具描述性的命名：
public void RegisterForNotifications()
{
    _service.DataReceived += OnDataReceived;
}

public void SetupListeners()
{
    _sensor.ValueChanged += OnSensorValueChanged;
}

// 如果订阅特定事件：
public void SubscribeToClickEvent(Button button)
{
    button.Click += OnButtonClick;
}

// ❌ 避免的命名：
public void AddEvents()                    // 太模糊
public void DoSubscribe()                  // 不专业
```



## 4. 取消订阅事件的函数

```C#
// ✅ 正确示例：
public void UnsubscribeFromEvents()
{
    _publisher.Event1 -= Handler1;
    _publisher.Event2 -= Handler2;
}

public void RemoveEventHandlers()
{
    _button.Click -= OnButtonClick;
    _textBox.TextChanged -= OnTextChanged;
}

public void CleanupEventHandlers()
{
    // 清理所有事件处理程序
    _service.DataReceived -= OnDataReceived;
}

// 特定场景：
public void DetachEventListeners()
{
    _model.PropertyChanged -= OnPropertyChanged;
}

public void DisposeEventSubscriptions()
{
    foreach (var subscription in _subscriptions)
    {
        subscription.Dispose();
    }
}

// ❌ 避免的命名：
public void RemoveEvents()                 // 不明确
public void DeleteHandlers()               // 不准确
```



## 5. 订阅者中事件处理方法

多种常见模式（选择一种并保持一致）：

```C#
// 模式1：On + 事件源 + 事件名 (推荐)
private void OnButtonClick(object sender, EventArgs e)
private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)

// 模式2：处理者后缀 (也很常见)
private void Button_ClickHandler(object sender, EventArgs e)
private void TextBox_TextChangedHandler(object sender, TextChangedEventArgs e)

// 模式3：Handle前缀
private void HandleButtonClick(object sender, EventArgs e)
private void HandleTextChanged(object sender, TextChangedEventArgs e)

// 模式4：事件源_事件名 (WPF自动生成的模式)
private void Button_Click(object sender, RoutedEventArgs e)
private void TextBox_TextChanged(object sender, TextChangedEventArgs e)

// 模式5：描述性命名（当方法做特定事情时）
private void UpdateUIWhenDataChanges(object sender, EventArgs e)
private void ValidateInputOnTextChange(object sender, TextChangedEventArgs e)

// ❌ 避免的命名：
private void Method1(object sender, EventArgs e)  // 无意义名称
private void abc(object sender, EventArgs e)      // 不清晰
```



## 6. 发布者和订阅者

### 发布者 (Publisher)：

```C#
// 根据角色命名：
public class Button : Control              // UI控件作为发布者
public class DataService                   // 服务作为发布者
public class TemperatureSensor             // 设备模拟作为发布者
public class MessageBroker                 // 消息中介作为发布者
public class EventAggregator               // 事件聚合器作为发布者

// 命名要点：
// 1. 名词或名词短语
// 2. 描述其功能或角色
// 3. 通常不需要"Publisher"后缀
```



### 订阅者 (Subscriber)：

```C#
// 根据角色命名：
public class MainWindow : Window           // UI窗口作为订阅者
public class ViewModel                     // ViewModel作为订阅者
public class NotificationService           // 通知服务作为订阅者
public class Logger                        // 日志记录器作为订阅者
public class DataProcessor                 // 数据处理者作为订阅者

// 专门的事件监听器：
public class EventListener                 // 通用监听器
public class ButtonClickMonitor            // 特定事件监听器
public class PropertyChangeObserver        // 属性变化观察者

// 命名要点：
// 1. 名词或名词短语
// 2. 描述其功能或角色
// 3. 通常不需要"Subscriber"后缀
```



## 7. 其他相关命名规范

### 事件参数类 (EventArgs)：

```C#
// 必须继承自 EventArgs，以 EventArgs 结尾
public class ClickEventArgs : EventArgs
public class TextChangedEventArgs : EventArgs
public class PropertyChangedEventArgs : EventArgs
public class ValueChangedEventArgs<T> : EventArgs

// 自定义路由事件参数：
public class BubbleClickEventArgs : RoutedEventArgs
```



### 委托 (Delegate)：

```C#
// 以 EventHandler 结尾
public delegate void ClickEventHandler(object sender, EventArgs e);
public delegate void TextChangedEventHandler(object sender, TextChangedEventArgs e);

// 泛型版本：
public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);
```
