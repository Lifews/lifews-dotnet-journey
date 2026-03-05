namespace Design.StrategyPattern.PayStrategy;

// 上下文类 - 支付处理器
public class PaymentProcessor
{
    private IPaymentStrategy _paymentStrategy;
    private decimal _amount;

    public PaymentProcessor(decimal amount)
    {
        _amount = amount;
    }

    // 设置支付策略
    public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy;
        Console.WriteLine($"已设置支付方式: {paymentStrategy.GetPaymentMethod()}");
    }

    // 执行支付
    public void ProcessPayment()
    {
        if (_paymentStrategy == null)
        {
            throw new InvalidOperationException("请先设置支付策略");
        }

        Console.WriteLine($"开始处理支付，金额: {_amount:C}");
        _paymentStrategy.Pay(_amount);
    }

    // 获取当前支付方式
    public string GetCurrentPaymentMethod()
    {
        return _paymentStrategy?.GetPaymentMethod() ?? "未设置支付方式";
    }
}

// 演示策略模式的使用
public class ShoppingCart
{
    private List<decimal> _items = new List<decimal>();
    private PaymentProcessor _paymentProcessor;

    public void AddItem(decimal price)
    {
        _items.Add(price);
        Console.WriteLine($"添加商品: {price:C}");
    }

    public decimal GetTotal()
    {
        decimal total = 0;
        foreach (var item in _items)
        {
            total += item;
        }
        return total;
    }

    public void Checkout(IPaymentStrategy paymentStrategy)
    {
        decimal total = GetTotal();
        Console.WriteLine($"\n=== 结算开始 ===");
        Console.WriteLine($"商品总数: {_items.Count}");
        Console.WriteLine($"总计金额: {total:C}");

        _paymentProcessor = new PaymentProcessor(total);
        _paymentProcessor.SetPaymentStrategy(paymentStrategy);
        _paymentProcessor.ProcessPayment();

        Console.WriteLine("=== 结算完成 ===\n");
    }
}
