using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerPrefsCurrencyRepository : ICurrencyRepository
{
    private readonly string _keyPrefix;
    private readonly string _existsKey;

    public PlayerPrefsCurrencyRepository(string userKey = "")
    {
        var prefix = string.IsNullOrEmpty(userKey) ? "" : userKey + "_";
        _keyPrefix = prefix + "Currency_";
        _existsKey = prefix + "Currency_Initialized";
    }

    public UniTaskVoid Save(CurrencyData currencyData)
    {
        foreach (var pair in currencyData.Currencies)
        {
            PlayerPrefs.SetFloat($"{_keyPrefix}{pair.Key}", pair.Value);
        }
        PlayerPrefs.SetInt(_existsKey, 1);
        PlayerPrefs.Save();
        
        return default(UniTaskVoid);
    }

    public UniTask<CurrencyData> Load()
    {
        var data = new CurrencyData();
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            data.SetAmount(type, PlayerPrefs.GetFloat($"{_keyPrefix}{type}", 0f));
        }
        return UniTask.FromResult(data);
    }

    public void Delete()
    {
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            PlayerPrefs.DeleteKey($"{_keyPrefix}{type}");
        }
        PlayerPrefs.DeleteKey(_existsKey);
        PlayerPrefs.Save();
    }

    public bool Exists() => PlayerPrefs.HasKey(_existsKey);
}
