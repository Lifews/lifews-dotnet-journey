// 举例1

namespace Design.StrategyPattern.PayStrategy;

public interface IPaymentStrategy
{
    void Pay(decimal amount);
    string GetPaymentMethod();
}
