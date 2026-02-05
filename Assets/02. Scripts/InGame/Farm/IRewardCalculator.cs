public interface IRewardCalculator
{
    double CalculateClickReward(CurrencyType currencyType);
    double CalculateAutoReward(CurrencyType currencyType);
}
