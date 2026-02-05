using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager s_instance;
    public static CurrencyManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("CurrencyManager");
                s_instance = go.AddComponent<CurrencyManager>();
                DontDestroyOnLoad(go);
            }
            return s_instance;
        }
    }

    private Dictionary<CurrencyType, double> _currencies = new();

    private bool _hasChanges = false;
    private float _lastSaveTime = 0f;
    private const float SAVE_INTERVAL = 5f;

    public static event Action<CurrencyType, double> OnCurrencyChanged;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            _lastSaveTime = Time.time;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (UserDataManager.Instance != null && UserDataManager.Instance.CurrencyData != null)
        {
            InitializeFromUserData();
        }
        else
        {
            UserDataManager.OnDataLoaded += InitializeFromUserData;
        }
    }

    private void Update()
    {
        if (_hasChanges && (Time.time - _lastSaveTime) >= SAVE_INTERVAL)
        {
            SaveData();
        }
    }

    private void SaveData()
    {
        var data = new CurrencyData();
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            data.SetAmount(type, (float)_currencies[type]);
        }
        UserDataManager.Instance.SaveCurrency(data);
        _hasChanges = false;
        _lastSaveTime = Time.time;
        Debug.Log($"[CurrencyManager] Batch save completed at {Time.time:F1}");
    }

    private void OnDestroy()
    {
        UserDataManager.OnDataLoaded -= InitializeFromUserData;
        
        if (_hasChanges)
        {
            SaveData();
            Debug.Log("[CurrencyManager] Final save on destroy");
        }
    }

    private void InitializeFromUserData()
    {
        var currencyData = UserDataManager.Instance?.CurrencyData;
        
        Debug.Log($"[CurrencyManager] InitializeFromUserData called, CurrencyData is null: {currencyData == null}");

        if (currencyData != null)
        {
            Debug.Log($"[CurrencyManager] Loading currency data with {currencyData.Currencies.Count} entries");
            foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
            {
                double amount = currencyData.GetAmount(type);
                _currencies[type] = amount;
                OnCurrencyChanged?.Invoke(type, amount);
                Debug.Log($"[CurrencyManager] Loaded {type}: {amount}");
            }
        }
        else
        {
            Debug.LogWarning("[CurrencyManager] CurrencyData is null, setting default values");
            foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
            {
                _currencies[type] = 0;
                OnCurrencyChanged?.Invoke(type, 0);
            }
        }
    }

    public double GetCurrency(CurrencyType type) => _currencies[type];

    public void AddCurrency(CurrencyType type, double amount)
    {
        if(amount <= 0) return;
        
        _currencies[type] = System.Math.Max(0, _currencies[type] + amount);
        OnCurrencyChanged?.Invoke(type, _currencies[type]);
        
        _hasChanges = true;
    }

    public bool TrySpendCurrency(CurrencyType type, double amount)
    {
        if (_currencies[type] >= amount)
        {
            _currencies[type] -= amount;
            OnCurrencyChanged?.Invoke(type, _currencies[type]);
            
            _hasChanges = true;
            
            return true;
        }
        return false;
    }

    public void SetCurrency(CurrencyType type, double amount)
    {
        _currencies[type] = System.Math.Max(0, amount);
        OnCurrencyChanged?.Invoke(type, _currencies[type]);
        
        _hasChanges = true;
    }

    public void ForceSave()
    {
        if (_hasChanges)
        {
            SaveData();
            Debug.Log("[CurrencyManager] Force save executed");
        }
    }
}
