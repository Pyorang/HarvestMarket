using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private Dictionary<UpgradeType, Upgrade> _upgrades = new();

    private UpgradeData _upgradeData;

    public static event Action<UpgradeType, int> OnUpgraded;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        InitializeUpgrades();
        LoadUpgradeData();
    }

    private void InitializeUpgrades()
    {
        _upgrades[UpgradeType.ChickenCoop] = new Upgrade(
            type: UpgradeType.ChickenCoop,
            name: "Chicken Coop",
            description: "+1 Chicken (+0.5 Eggs/sec)",
            maxLevel: 100,
            baseCost: 50f,
            costMultiplier: 1.15f,
            baseValue: 0f,
            valueMultiplier: 0.5f
        );

        _upgrades[UpgradeType.Pigpen] = new Upgrade(
            type: UpgradeType.Pigpen,
            name: "Pigpen",
            description: "+1 Pig (+0.2 Meat/sec)",
            maxLevel: 100,
            baseCost: 300f,
            costMultiplier: 1.15f,
            baseValue: 0f,
            valueMultiplier: 0.2f
        );

        _upgrades[UpgradeType.CattleBarn] = new Upgrade(
            type: UpgradeType.CattleBarn,
            name: "Cattle Barn",
            description: "+1 Cow (+0.08 Milk/sec)",
            maxLevel: 100,
            baseCost: 1500f,
            costMultiplier: 1.15f,
            baseValue: 0f,
            valueMultiplier: 0.08f
        );

        _upgrades[UpgradeType.EggBasket] = new Upgrade(
            type: UpgradeType.EggBasket,
            name: "Egg Basket",
            description: "+1 Egg per click",
            maxLevel: 100,
            baseCost: 30f,
            costMultiplier: 1.20f,
            baseValue: 1f,
            valueMultiplier: 1f
        );

        _upgrades[UpgradeType.ButcherKnife] = new Upgrade(
            type: UpgradeType.ButcherKnife,
            name: "Butcher Knife",
            description: "+1 Meat per click",
            maxLevel: 100,
            baseCost: 200f,
            costMultiplier: 1.20f,
            baseValue: 1f,
            valueMultiplier: 1f
        );

        _upgrades[UpgradeType.MilkBucket] = new Upgrade(
            type: UpgradeType.MilkBucket,
            name: "Milk Bucket",
            description: "+1 Milk per click",
            maxLevel: 100,
            baseCost: 1000f,
            costMultiplier: 1.20f,
            baseValue: 1f,
            valueMultiplier: 1f
        );
    }

    private void LoadUpgradeData()
    {
        _upgradeData = UserDataManager.Instance.GetUserData<UpgradeData>();
        
        if (_upgradeData == null)
        {
            Debug.LogError("[UpgradeManager] UpgradeData not found in UserDataManager!");
        }
    }

    public Upgrade GetUpgrade(UpgradeType type)
    {
        return _upgrades.TryGetValue(type, out var upgrade) ? upgrade : null;
    }

    public int GetCurrentLevel(UpgradeType type)
    {
        return _upgradeData?.GetLevel(type) ?? 0;
    }

    public bool TryUpgrade(UpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        if (upgrade == null) return false;

        int currentLevel = GetCurrentLevel(type);
        float gold = (float)ResourceManager.Instance.GetResource(ResourceType.Gold);

        if (!upgrade.CanUpgrade(currentLevel, gold))
            return false;

        float cost = upgrade.GetCost(currentLevel);

        if (!ResourceManager.Instance.TrySpendResource(ResourceType.Gold, cost))
            return false;

        _upgradeData.AddLevel(type);
        UserDataManager.Instance.SaveUserData();

        OnUpgraded?.Invoke(type, GetCurrentLevel(type));
        return true;
    }
}
