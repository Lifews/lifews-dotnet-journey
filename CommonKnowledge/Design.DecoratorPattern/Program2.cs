// 通过委托方式来实现装饰器模式

#if false

var sender = new MessageSender("Tom");

Action<string> sendAction = sender.Send;

sendAction("Hello World !");

// 装饰器模式的核心是在不改变原始对象的情况下，动态添加行为。
// 通过委托实现时，我们应当在返回的委托中先执行额外逻辑，再调用原始委托（或反之）。
// 例如，实现一个日志装饰器：
Func<Action<string>, Action<string>> loggingDecorator = action =>
{
    return message =>
    {
        Console.WriteLine($"Logging: about to send message: {message}");
        action(message); // 调用原始委托
        Console.WriteLine("Logging: message sent");
    };
};

var decoratedSend = loggingDecorator(sendAction);
decoratedSend("Hello");

public class MessageSender
{
    private readonly string _name;

    public MessageSender(string name)
    {
        _name = name;
    }

    public void Send(string message)
    {
        Console.WriteLine($"Sending messqge from  {_name}: {message} ");
    }
}

#endif
