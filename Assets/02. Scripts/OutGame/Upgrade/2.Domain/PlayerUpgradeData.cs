using System;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class PlayerUpgradeData
{
    [FirestoreProperty]
    public Dictionary<string, int> UpgradeLevels { get; set; } = new();

    public void SetDefault()
    {
        UpgradeLevels.Clear();
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            UpgradeLevels[type.ToString()] = 0;
        }
    }

    public int GetLevel(UpgradeType type)
    {
        string key = type.ToString();
        return UpgradeLevels.TryGetValue(key, out int level) ? level : 0;
    }

    public void SetLevel(UpgradeType type, int level)
    {
        UpgradeLevels[type.ToString()] = level;
    }

    public void AddLevel(UpgradeType type, int amount = 1)
    {
        string key = type.ToString();
        if (UpgradeLevels.ContainsKey(key))
            UpgradeLevels[key] += amount;
        else
            UpgradeLevels[key] = amount;
    }
}
