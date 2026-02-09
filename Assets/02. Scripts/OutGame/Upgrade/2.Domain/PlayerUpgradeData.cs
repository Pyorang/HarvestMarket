#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
#endif
using System;
using System.Collections.Generic;

#if !UNITY_WEBGL || UNITY_EDITOR
[FirestoreData]
#endif
public class PlayerUpgradeData
{
#if !UNITY_WEBGL || UNITY_EDITOR
    [FirestoreProperty]
#endif
    public Dictionary<string, int> UpgradeLevels { get; set; } = new();

#if !UNITY_WEBGL || UNITY_EDITOR
    [FirestoreProperty]
#endif
    public long LastSavedAt { get; set; } = 0;

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
