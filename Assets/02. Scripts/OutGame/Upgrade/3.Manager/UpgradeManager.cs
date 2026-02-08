using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }


    private IUpgradeRepository _upgradeRepository;
    private Dictionary<UpgradeType, Upgrade> _upgrades = new();
    private PlayerUpgradeData _upgradeData;

    public bool IsInitialized { get; private set; }

    public static event Action<UpgradeType, int> OnUpgraded;
    public static event Action OnInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeRepository();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeRepository()
    {
        _upgradeRepository = new HybridUpgradeRepository(this);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            (_upgradeRepository as HybridUpgradeRepository)?.FlushToRemote();
    }

    private void OnApplicationQuit()
    {
        (_upgradeRepository as HybridUpgradeRepository)?.FlushToRemote();
    }

    private async void Start()
    {
        await InitializeAsync();
    }

    private async UniTask InitializeAsync()
    {
        try
        {
            await LoadUpgradeDataAsync();
            await InitializeUpgradesAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"[UpgradeManager] Initialization failed: {e.Message}");
        }
    }

    private async UniTask LoadUpgradeDataAsync()
    {
        _upgradeData = await _upgradeRepository.Load();
        Debug.Log("[UpgradeManager] Upgrade data loaded");
    }

    private async UniTask InitializeUpgradesAsync()
    {
        var tcs = new UniTaskCompletionSource<List<UpgradeSpec>>();
        
        UpgradeSpecLoader.LoadAsync(specs =>
        {
            tcs.TrySetResult(specs);
        });
        
        var specList = await tcs.Task;
        
        foreach (var spec in specList)
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
    }

    public Upgrade GetUpgrade(UpgradeType type)
    {
        return _upgrades.TryGetValue(type, out var upgrade) ? upgrade : null;
    }

    public IEnumerable<Upgrade> GetAllUpgrades()
    {
        return _upgrades.Values;
    }

    public async UniTask<bool> TryUpgradeAsync(UpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        if (upgrade == null) return false;

        float currentGold = (float)CurrencyManager.Instance.GetCurrency(CurrencyType.Gold);

        if (!upgrade.CanUpgrade(currentGold))
            return false;

        float cost = upgrade.GetCost();

        if (!CurrencyManager.Instance.TrySpendCurrency(CurrencyType.Gold, cost))
            return false;

        upgrade.LevelUp();
        _upgradeData.SetLevel(type, upgrade.CurrentLevel);
        
        try
        {
            _upgradeRepository.Save(_upgradeData).Forget();
            OnUpgraded?.Invoke(type, upgrade.CurrentLevel);
            Debug.Log($"[UpgradeManager] Upgraded {type} to level {upgrade.CurrentLevel}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[UpgradeManager] Failed to save upgrade: {e.Message}");
            upgrade.SetLevel(upgrade.CurrentLevel - 1);
            _upgradeData.SetLevel(type, upgrade.CurrentLevel);
            CurrencyManager.Instance.AddCurrency(CurrencyType.Gold, cost);
            return false;
        }
    }

    public bool CanUpgrade(UpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        if (upgrade == null) return false;

        float currentGold = (float)CurrencyManager.Instance.GetCurrency(CurrencyType.Gold);
        return upgrade.CanUpgrade(currentGold);
    }

    public bool TryUpgrade(UpgradeType type)
    {
        return TryUpgradeAsync(type).GetAwaiter().GetResult();
    }
}
