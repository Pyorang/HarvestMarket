using Cysharp.Threading.Tasks;

public interface ICurrencyRepository
{
    public UniTaskVoid Save(CurrencyData currencyData);
    public UniTask<CurrencyData> Load();
}