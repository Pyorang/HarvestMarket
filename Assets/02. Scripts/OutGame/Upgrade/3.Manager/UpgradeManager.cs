using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private Dictionary<UpgradeType, Upgrade> _upgrades = new();
    private PlayerUpgradeData _upgradeData;

    public bool IsInitialized { get; private set; }

    public static event Action<UpgradeType, int> OnUpgraded;
    public static event Action OnInitialized;

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
        _upgradeData = UserDataManager.Instance.UpgradeData;

        if (_upgradeData == null)
            Debug.LogError("[UpgradeManager] PlayerUpgradeData not found!");
    }

    private void InitializeUpgrades()
    {
        UpgradeSpecLoader.LoadAsync(specs =>
        {
            foreach (var spec in specs)
            {
                int currentLevel = _upgradeData?.GetLevel(spec.Type) ?? 0;

                _upgrades[spec.Type] = new Upgrade(
                    type: spec.Type,
                    name: spec.Name,
                    description: spec.Description,
                    maxLevel: spec.MaxLevel,
                    baseCost: spec.BaseCost,
                    costMultiplier: spec.CostMultiplier,
                    baseValue: spec.BaseValue,
                    valueMultiplier: spec.ValueMultiplier,
                    currentLevel: currentLevel
                );
            }

            IsInitialized = true;
            OnInitialized?.Invoke();
            Debug.Log($"[UpgradeManager] Loaded {_upgrades.Count} upgrades from CSV");
        });
    }

    public Upgrade GetUpgrade(UpgradeType type)
    {
        return _upgrades.TryGetValue(type, out var upgrade) ? upgrade : null;
    }

    public IEnumerable<Upgrade> GetAllUpgrades()
    {
        return _upgrades.Values;
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
        UserDataManager.Instance.SaveUpgrade();

        OnUpgraded?.Invoke(type, upgrade.CurrentLevel);
        return true;
    }
}
