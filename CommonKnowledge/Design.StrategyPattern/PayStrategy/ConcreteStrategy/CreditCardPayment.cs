namespace Design.StrategyPattern.PayStrategy;

// 具体策略类：信用卡支付
public class CreditCardPayment : IPaymentStrategy
{
    private readonly string _cardNumber;

    public CreditCardPayment(string cardNumber)
    {
        _cardNumber = cardNumber;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"使用信用卡支付 {amount:C}");
        Console.WriteLine($"卡号: {MaskCardNumber(_cardNumber)}");
        // 这里可以添加实际的支付逻辑
        ProcessCreditCardPayment(amount);
    }

    public string GetPaymentMethod()
    {
        return "信用卡支付";
    }

    private string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
            return "****";

        return new string('*', cardNumber.Length - 4) + cardNumber.Substring(cardNumber.Length - 4);
    }

    private void ProcessCreditCardPayment(decimal amount)
    {
        // 模拟信用卡支付处理
        Console.WriteLine("正在验证信用卡信息...");
        Console.WriteLine("正在进行支付授权...");
        Console.WriteLine($"支付成功: {amount:C}");
        Console.WriteLine();
    }
}
