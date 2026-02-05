using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    private ICurrencyRepository _currencyRepository;
    public CurrencyData CurrencyData { get; private set; }
    
    public static event Action OnDataLoaded;

    private void Awake()
    {
        Instance = this;
    }

    public async UniTaskVoid Initialize(string email)
    {
        _currencyRepository = new FirebaseCurrencyRepository();

        try
        {
            var existingData = await _currencyRepository.Load();
            if (existingData != null && existingData.Currencies.Count > 0)
            {
                CurrencyData = existingData;
                Debug.Log("[UserDataManager] Currency data loaded from Firebase successfully");
            }
            else
            {
                SetDefaultAll();
                Debug.Log("[UserDataManager] No existing data, set to default");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase load failed: {e.Message}");
            SetDefaultAll();
        }
        
        OnDataLoaded?.Invoke();
        Debug.Log("[UserDataManager] Data loading completed, event fired");
    }

    public void SetDefaultAll()
    {
        CurrencyData = new CurrencyData();
        CurrencyData.SetDefault();
    }

    public async UniTask LoadAll()
    {
        CurrencyData = await _currencyRepository.Load();
    }

    public void SaveCurrency(CurrencyData data)
    {
        CurrencyData = data;
        _currencyRepository.Save(CurrencyData);
    }

    public void DeleteAll()
    {
        _currencyRepository.Delete();
        SetDefaultAll();
    }
}
