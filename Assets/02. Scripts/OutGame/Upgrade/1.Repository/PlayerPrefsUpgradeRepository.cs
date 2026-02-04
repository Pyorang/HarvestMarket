using System;
using UnityEngine;

public class PlayerPrefsUpgradeRepository : IRepository<PlayerUpgradeData>
{
    private readonly string _keyPrefix;
    private readonly string _existsKey;

    public PlayerPrefsUpgradeRepository(string userKey = "")
    {
        var prefix = string.IsNullOrEmpty(userKey) ? "" : userKey + "_";
        _keyPrefix = prefix + "Upgrade_";
        _existsKey = prefix + "Upgrade_Initialized";
    }

    public PlayerUpgradeData Load()
    {
        var data = new PlayerUpgradeData();
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            data.UpgradeLevels[type] = PlayerPrefs.GetInt($"{_keyPrefix}{type}", 0);
        }
        return data;
    }

    public void Save(PlayerUpgradeData data)
    {
        foreach (var pair in data.UpgradeLevels)
        {
            PlayerPrefs.SetInt($"{_keyPrefix}{pair.Key}", pair.Value);
        }
        PlayerPrefs.SetInt(_existsKey, 1);
        PlayerPrefs.Save();
    }

    public void Delete()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            PlayerPrefs.DeleteKey($"{_keyPrefix}{type}");
        }
        PlayerPrefs.DeleteKey(_existsKey);
        PlayerPrefs.Save();
    }

    public bool Exists() => PlayerPrefs.HasKey(_existsKey);
}
