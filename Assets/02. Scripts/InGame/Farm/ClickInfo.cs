public struct ClickInfo
{
    public CurrencyType CurrencyType;
    public double Amount;

    public ClickInfo(CurrencyType currencyType, double amount)
    {
        CurrencyType = currencyType;
        Amount = amount;
    }
}
