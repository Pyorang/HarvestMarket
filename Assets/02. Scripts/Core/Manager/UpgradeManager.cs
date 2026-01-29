using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private Dictionary<UpgradeType, Upgrade> _upgrades = new();

    private PlayerUpgradeData _upgradeData;

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
        LoadUpgradeData();
        InitializeUpgrades();
    }

    private void LoadUpgradeData()
    {
        _upgradeData = UserDataManager.Instance.GetUserData<PlayerUpgradeData>();

        if (_upgradeData == null)
            Debug.LogError("[UpgradeManager] PlayerUpgradeData not found in UserDataManager!");
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
            valueMultiplier: 0.5f,
            currentLevel: _upgradeData.GetLevel(UpgradeType.ChickenCoop)
        );

        _upgrades[UpgradeType.Pigpen] = new Upgrade(
            type: UpgradeType.Pigpen,
            name: "Pigpen",
            description: "+1 Pig (+0.2 Meat/sec)",
            maxLevel: 100,
            baseCost: 300f,
            costMultiplier: 1.15f,
            baseValue: 0f,
            valueMultiplier: 0.2f,
            currentLevel: _upgradeData.GetLevel(UpgradeType.Pigpen)
        );

        _upgrades[UpgradeType.CattleBarn] = new Upgrade(
            type: UpgradeType.CattleBarn,
            name: "Cattle Barn",
            description: "+1 Cow (+0.08 Milk/sec)",
            maxLevel: 100,
            baseCost: 1500f,
            costMultiplier: 1.15f,
            baseValue: 0f,
            valueMultiplier: 0.08f,
            currentLevel: _upgradeData.GetLevel(UpgradeType.CattleBarn)
        );

        _upgrades[UpgradeType.EggBasket] = new Upgrade(
            type: UpgradeType.EggBasket,
            name: "Egg Basket",
            description: "+1 Egg per click",
            maxLevel: 100,
            baseCost: 30f,
            costMultiplier: 1.20f,
            baseValue: 1f,
            valueMultiplier: 1f,
            currentLevel: _upgradeData.GetLevel(UpgradeType.EggBasket)
        );

        _upgrades[UpgradeType.ButcherKnife] = new Upgrade(
            type: UpgradeType.ButcherKnife,
            name: "Butcher Knife",
            description: "+1 Meat per click",
            maxLevel: 100,
            baseCost: 200f,
            costMultiplier: 1.20f,
            baseValue: 1f,
            valueMultiplier: 1f,
            currentLevel: _upgradeData.GetLevel(UpgradeType.ButcherKnife)
        );

        _upgrades[UpgradeType.MilkBucket] = new Upgrade(
            type: UpgradeType.MilkBucket,
            name: "Milk Bucket",
            description: "+1 Milk per click",
            maxLevel: 100,
            baseCost: 1000f,
            costMultiplier: 1.20f,
            baseValue: 1f,
            valueMultiplier: 1f,
            currentLevel: _upgradeData.GetLevel(UpgradeType.MilkBucket)
        );
    }

    public Upgrade GetUpgrade(UpgradeType type)
    {
        return _upgrades.TryGetValue(type, out var upgrade) ? upgrade : null;
    }

    public bool TryUpgrade(UpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        if (upgrade == null) return false;

        float currentGold = (float)ResourceManager.Instance.GetResource(ResourceType.Gold);

        if (!upgrade.CanUpgrade(currentGold))
            return false;

        float cost = upgrade.GetCost();

        if (!ResourceManager.Instance.TrySpendResource(ResourceType.Gold, cost))
            return false;

        upgrade.LevelUp();
        _upgradeData.SetLevel(type, upgrade.CurrentLevel);
        UserDataManager.Instance.SaveUserData();

        OnUpgraded?.Invoke(type, upgrade.CurrentLevel);
        return true;
    }
}
