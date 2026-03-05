namespace Design.StrategyPattern.PayStrategy;

// 具体策略类：支付宝支付
public class AlipayPayment : IPaymentStrategy
{
    private readonly string _account;

    public AlipayPayment(string account)
    {
        _account = account;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"使用支付宝支付 {amount:C}");
        Console.WriteLine($"账户: {_account}");
        // 模拟支付宝支付逻辑
        ProcessAlipayPayment(amount);
    }

    public string GetPaymentMethod()
    {
        return "支付宝支付";
    }

    private void ProcessAlipayPayment(decimal amount)
    {
        Console.WriteLine("正在跳转到支付宝...");
        Console.WriteLine("请确认支付...");
        Console.WriteLine($"支付成功: {amount:C}");
        Console.WriteLine();
    }
}