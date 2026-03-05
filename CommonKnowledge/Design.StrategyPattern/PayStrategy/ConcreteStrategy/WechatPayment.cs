namespace Design.StrategyPattern.PayStrategy;

// 具体策略类：微信支付
public class WechatPayment : IPaymentStrategy
{
    private readonly string _openId;

    public WechatPayment(string openId)
    {
        _openId = openId;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"使用微信支付 {amount:C}");
        Console.WriteLine($"OpenID: {_openId}");
        ProcessWechatPayment(amount);
    }

    public string GetPaymentMethod()
    {
        return "微信支付";
    }

    private void ProcessWechatPayment(decimal amount)
    {
        Console.WriteLine("正在调用微信支付API...");
        Console.WriteLine("等待用户确认...");
        Console.WriteLine($"支付成功: {amount:C}");
        Console.WriteLine();
    }
}
