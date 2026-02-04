using UnityEngine;
using Cysharp.Threading.Tasks;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    private ICurrencyRepository _currencyRepository;
    private IRepository<PlayerUpgradeData> _upgradeRepository;

    public CurrencyData CurrencyData { get; private set; }
    public PlayerUpgradeData UpgradeData { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async UniTaskVoid Initialize(string email)
    {
        _currencyRepository = new PlayerPrefsCurrencyRepository(email);
        // Firebase 사용시: _currencyRepository = new FirebaseCurrencyRepository();
        _upgradeRepository = new PlayerPrefsUpgradeRepository(email);

        // PlayerPrefs의 경우 Exists() 동기 메서드 사용 가능
        if (_currencyRepository is PlayerPrefsCurrencyRepository playerPrefsRepo && playerPrefsRepo.Exists())
        {
            await LoadAll();
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

    public async UniTask LoadAll()
    {
        CurrencyData = await _currencyRepository.Load();
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
        // PlayerPrefs의 경우만 Delete 메서드 사용 가능
        if (_currencyRepository is PlayerPrefsCurrencyRepository playerPrefsRepo)
        {
            playerPrefsRepo.Delete();
        }
        _upgradeRepository.Delete();
        SetDefaultAll();
    }
}
