using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerUpgradeData : IUserData
{
    public Dictionary<UpgradeType, int> UpgradeLevels { get; private set; } = new();

    public void SetDefaultData()
    {
        UpgradeLevels.Clear();
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            UpgradeLevels[type] = 0;
        }
    }

    public bool LoadData()
    {
        UpgradeLevels.Clear();
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            UpgradeLevels[type] = PlayerPrefs.GetInt($"Upgrade_{type}", 0);
        }
        return true;
    }

    public bool SaveData()
    {
        foreach (var pair in UpgradeLevels)
        {
            PlayerPrefs.SetInt($"Upgrade_{pair.Key}", pair.Value);
        }
        PlayerPrefs.Save();
        return true;
    }

    public int GetLevel(UpgradeType type) => UpgradeLevels.TryGetValue(type, out int level) ? level : 0;
    
    public void SetLevel(UpgradeType type, int level) => UpgradeLevels[type] = level;
    
    public void AddLevel(UpgradeType type, int amount = 1) => UpgradeLevels[type] += amount;
}
