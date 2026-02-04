using System;
using UnityEngine;

public class PlayerPrefsCurrencyRepository : IRepository<CurrencyData>
{
    private readonly string _keyPrefix;
    private readonly string _existsKey;

    public PlayerPrefsCurrencyRepository(string userKey = "")
    {
        var prefix = string.IsNullOrEmpty(userKey) ? "" : userKey + "_";
        _keyPrefix = prefix + "Currency_";
        _existsKey = prefix + "Currency_Initialized";
    }

    public CurrencyData Load()
    {
        var data = new CurrencyData();
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            data.Currencies[type] = PlayerPrefs.GetFloat($"{_keyPrefix}{type}", 0f);
        }
        return data;
    }

    public void Save(CurrencyData data)
    {
        foreach (var pair in data.Currencies)
        {
            PlayerPrefs.SetFloat($"{_keyPrefix}{pair.Key}", pair.Value);
        }
        PlayerPrefs.SetInt(_existsKey, 1);
        PlayerPrefs.Save();
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
