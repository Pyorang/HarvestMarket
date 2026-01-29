using System;

[Serializable]
public class Upgrade
{
    // 기본 정보
    public UpgradeType Type { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    // 레벨 관련
    public int MaxLevel { get; private set; }
    
    // 비용 관련
    public float BaseCost { get; private set; }
    public float CostMultiplier { get; private set; }
    
    // 효과 관련
    public float BaseValue { get; private set; }
    public float ValueMultiplier { get; private set; }

    public Upgrade(
        UpgradeType type,
        string name,
        string description,
        int maxLevel,
        float baseCost,
        float costMultiplier,
        float baseValue,
        float valueMultiplier)
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

        Type = type;
        Name = name;
        Description = description;
        MaxLevel = maxLevel;
        BaseCost = baseCost;
        CostMultiplier = costMultiplier;
        BaseValue = baseValue;
        ValueMultiplier = valueMultiplier;
    }

    public float GetCost(int currentLevel)
    {
        if (currentLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(currentLevel), "Level cannot be negative.");
        
        if (currentLevel >= MaxLevel) return -1f;
        
        if (currentLevel == 0) return BaseCost;

        return BaseCost + MathF.Pow(CostMultiplier, currentLevel);
    }

    public float GetTotalValue(int currentLevel)
    {
        if (currentLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(currentLevel), "Level cannot be negative.");
        
        if (currentLevel == 0) return BaseValue;
        
        return BaseValue + (currentLevel * ValueMultiplier);
    }

    public float GetNextLevelAddedValue(int currentLevel)
    {
        if (currentLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(currentLevel), "Level cannot be negative.");
        
        if (currentLevel >= MaxLevel) return 0f;
        
        return GetTotalValue(currentLevel + 1) - GetTotalValue(currentLevel);
    }

    public bool CanUpgrade(int currentLevel, float currentGold)
    {
        if (currentLevel < 0 || currentGold < 0) return false;
        if (currentLevel >= MaxLevel) return false;
        
        return currentGold >= GetCost(currentLevel);
    }

    public bool IsMaxLevel(int currentLevel)
    {
        return currentLevel >= MaxLevel;
    }
}
