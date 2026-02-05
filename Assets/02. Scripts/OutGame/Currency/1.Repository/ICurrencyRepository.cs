using Cysharp.Threading.Tasks;

public interface ICurrencyRepository
{
    UniTaskVoid Save(CurrencyData currencyData);
    UniTask<CurrencyData> Load();
    UniTaskVoid Delete();
}