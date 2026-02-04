using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    private IRepository<CurrencyData> _currencyRepository;
    private IRepository<PlayerUpgradeData> _upgradeRepository;

    public CurrencyData CurrencyData { get; private set; }
    public PlayerUpgradeData UpgradeData { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(string email)
    {
        _currencyRepository = new PlayerPrefsCurrencyRepository(email);
        _upgradeRepository = new PlayerPrefsUpgradeRepository(email);

        if (_currencyRepository.Exists())
        {
            LoadAll();
        }
        else
        {
            SetDefaultAll();
        }
    }

    public void SetDefaultAll()
    {
        CurrencyData = new CurrencyData();
        CurrencyData.SetDefault();

        UpgradeData = new PlayerUpgradeData();
        UpgradeData.SetDefault();
    }

    public void LoadAll()
    {
        CurrencyData = _currencyRepository.Load();
        UpgradeData = _upgradeRepository.Load();
    }

    public void SaveAll()
    {
        foreach (CurrencyType type in System.Enum.GetValues(typeof(CurrencyType)))
        {
            CurrencyData.Currencies[type] = (float)CurrencyManager.Instance.GetCurrency(type);
        }

        _currencyRepository.Save(CurrencyData);
        _upgradeRepository.Save(UpgradeData);
    }

    public void SaveCurrency()
    {
        foreach (CurrencyType type in System.Enum.GetValues(typeof(CurrencyType)))
        {
            CurrencyData.Currencies[type] = (float)CurrencyManager.Instance.GetCurrency(type);
        }
        _currencyRepository.Save(CurrencyData);
    }

    public void SaveUpgrade()
    {
        _upgradeRepository.Save(UpgradeData);
    }

    public void DeleteAll()
    {
        _currencyRepository.Delete();
        _upgradeRepository.Delete();
        SetDefaultAll();
    }
}
