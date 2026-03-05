namespace Design.StrategyPattern.DiscountStrategy;

// 折扣策略接口
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal originalPrice);
    string GetDiscountDescription();
}