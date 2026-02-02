using System;
using System.Collections.Generic;

public class PlayerUpgradeData
{
    public Dictionary<UpgradeType, int> UpgradeLevels { get; set; } = new();

    public void SetDefault()
    {
        UpgradeLevels.Clear();
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            UpgradeLevels[type] = 0;
        }
    }

    public int GetLevel(UpgradeType type) => UpgradeLevels.TryGetValue(type, out int level) ? level : 0;

    public void SetLevel(UpgradeType type, int level) => UpgradeLevels[type] = level;

    public void AddLevel(UpgradeType type, int amount = 1) => UpgradeLevels[type] += amount;
}
