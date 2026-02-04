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

    public static event Action<CurrencyType, double> OnCurrencyChanged;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeFromUserData();
    }

    private void InitializeFromUserData()
    {
        var currencyData = UserDataManager.Instance.CurrencyData;

        if (currencyData != null)
        {
            foreach (var pair in currencyData.Currencies)
            {
                _currencies[pair.Key] = pair.Value;
                OnCurrencyChanged?.Invoke(pair.Key, pair.Value);
            }
        }
        else
        {
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
    }

    public bool TrySpendCurrency(CurrencyType type, double amount)
    {
        if (_currencies[type] >= amount)
        {
            _currencies[type] -= amount;
            OnCurrencyChanged?.Invoke(type, _currencies[type]);
            return true;
        }
        return false;
    }

    public void SetCurrency(CurrencyType type, double amount)
    {
        _currencies[type] = System.Math.Max(0, amount);
        OnCurrencyChanged?.Invoke(type, _currencies[type]);
    }
}
