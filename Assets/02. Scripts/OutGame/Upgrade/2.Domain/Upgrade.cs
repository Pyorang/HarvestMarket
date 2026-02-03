using System;

[Serializable]
public class Upgrade
{
    public UpgradeType Type { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int MaxLevel { get; private set; }
    public float BaseCost { get; private set; }
    public float CostMultiplier { get; private set; }
    public float BaseValue { get; private set; }
    public float ValueMultiplier { get; private set; }
    public int CurrentLevel { get; private set; }

    public Upgrade(
        UpgradeType type,
        string name,
        string description,
        int maxLevel,
        float baseCost,
        float costMultiplier,
        float baseValue,
        float valueMultiplier,
        int currentLevel = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));

        if (maxLevel <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxLevel), "MaxLevel must be greater than 0.");

        if (baseCost < 0)
            throw new ArgumentOutOfRangeException(nameof(baseCost), "BaseCost cannot be negative.");

        if (costMultiplier <= 0)
            throw new ArgumentOutOfRangeException(nameof(costMultiplier), "CostMultiplier must be greater than 0.");

        if (baseValue < 0)
            throw new ArgumentOutOfRangeException(nameof(baseValue), "BaseValue cannot be negative.");

        if (valueMultiplier < 0)
            throw new ArgumentOutOfRangeException(nameof(valueMultiplier), "ValueMultiplier cannot be negative.");

        if (currentLevel < 0 || currentLevel > maxLevel)
            throw new ArgumentOutOfRangeException(nameof(currentLevel), "CurrentLevel must be between 0 and MaxLevel.");

        Type = type;
        Name = name;
        Description = description;
        MaxLevel = maxLevel;
        BaseCost = baseCost;
        CostMultiplier = costMultiplier;
        BaseValue = baseValue;
        ValueMultiplier = valueMultiplier;
        CurrentLevel = currentLevel;
    }

    public float GetCost()
    {
        if (CurrentLevel >= MaxLevel) return -1f;
        return BaseCost + MathF.Pow(CostMultiplier, CurrentLevel);
    }

    public float GetTotalValue()
    {
        if (CurrentLevel == 0) return BaseValue;
        return BaseValue + (CurrentLevel * ValueMultiplier);
    }

    public bool CanUpgrade(float currentGold)
    {
        if (CurrentLevel >= MaxLevel) return false;
        return currentGold >= GetCost();
    }

    public bool IsMaxLevel()
    {
        return CurrentLevel >= MaxLevel;
    }

    public void LevelUp()
    {
        if (CurrentLevel < MaxLevel)
            CurrentLevel++;
    }

    public void SetLevel(int level)
    {
        if (level < 0) level = 0;
        if (level > MaxLevel) level = MaxLevel;
        CurrentLevel = level;
    }
}
