public class UpgradeRewardCalculator : IRewardCalculator
{
    public double CalculateClickReward(CurrencyType currencyType)
    {
        var upgradeType = GetClickUpgradeType(currencyType);
        var upgrade = UpgradeManager.Instance.GetUpgrade(upgradeType);
        return (upgrade?.GetTotalValue() ?? 0) * FeverManager.Instance.BonusMultiplier;
    }

    public double CalculateAutoReward(CurrencyType currencyType)
    {
        var upgradeType = GetAutoUpgradeType(currencyType);
        var upgrade = UpgradeManager.Instance.GetUpgrade(upgradeType);
        return (upgrade?.GetTotalValue() ?? 0) * FeverManager.Instance.BonusMultiplier;
    }

    private UpgradeType GetClickUpgradeType(CurrencyType type) => type switch
    {
        CurrencyType.Egg => UpgradeType.EggBasket,
        CurrencyType.Meat => UpgradeType.ButcherKnife,
        CurrencyType.Milk => UpgradeType.MilkBucket,
        _ => UpgradeType.EggBasket
    };

    private UpgradeType GetAutoUpgradeType(CurrencyType type) => type switch
    {
        CurrencyType.Egg => UpgradeType.ChickenCoop,
        CurrencyType.Meat => UpgradeType.Pigpen,
        CurrencyType.Milk => UpgradeType.CattleBarn,
        _ => UpgradeType.ChickenCoop
    };
}
