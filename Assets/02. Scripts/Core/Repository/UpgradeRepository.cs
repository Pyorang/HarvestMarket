using System;
using UnityEngine;

public class UpgradeRepository : IRepository<PlayerUpgradeData>
{
    private const string KEY_PREFIX = "Upgrade_";
    private const string EXISTS_KEY = "Upgrade_Initialized";

    public PlayerUpgradeData Load()
    {
        var data = new PlayerUpgradeData();
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            data.UpgradeLevels[type] = PlayerPrefs.GetInt($"{KEY_PREFIX}{type}", 0);
        }
        return data;
    }

    public void Save(PlayerUpgradeData data)
    {
        foreach (var pair in data.UpgradeLevels)
        {
            PlayerPrefs.SetInt($"{KEY_PREFIX}{pair.Key}", pair.Value);
        }
        PlayerPrefs.SetInt(EXISTS_KEY, 1);
        PlayerPrefs.Save();
    }

    public void Delete()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            PlayerPrefs.DeleteKey($"{KEY_PREFIX}{type}");
        }
        PlayerPrefs.DeleteKey(EXISTS_KEY);
        PlayerPrefs.Save();
    }

    public bool Exists() => PlayerPrefs.HasKey(EXISTS_KEY);
}
