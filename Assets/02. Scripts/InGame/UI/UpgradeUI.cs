using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private UpgradeType _upgradeType;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private GameObject _lockImage;

    private Upgrade _upgrade;

    private void OnEnable()
    {
        ResourceManager.OnResourceChanged += OnResourceChanged;
        UpgradeManager.OnUpgraded += OnUpgraded;

        RefreshUI();
    }

    private void OnDisable()
    {
        ResourceManager.OnResourceChanged -= OnResourceChanged;
        UpgradeManager.OnUpgraded -= OnUpgraded;
    }

    private void OnResourceChanged(ResourceType type, double amount)
    {
        if (type == ResourceType.Gold)
        {
            RefreshButtonState();
        }
    }

    private void OnUpgraded(UpgradeType type, int newLevel)
    {
        if (type == _upgradeType)
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        _upgrade = UpgradeManager.Instance.GetUpgrade(_upgradeType);
        if (_upgrade == null) return;

        int currentLevel = UpgradeManager.Instance.GetCurrentLevel(_upgradeType);

        _titleText.text = $"{_upgrade.Name} (Lv. {currentLevel})";

        if (_upgrade.IsMaxLevel(currentLevel))
        {
            _costText.text = "MAX";
        }
        else
        {
            float cost = _upgrade.GetCost(currentLevel);
            _costText.text = $"{((double)cost).ToFormattedString()}";
        }

        RefreshButtonState();
    }

    private void RefreshButtonState()
    {
        if (_upgrade == null || _upgradeButton == null) return;

        int currentLevel = UpgradeManager.Instance.GetCurrentLevel(_upgradeType);
        float currentGold = (float)ResourceManager.Instance.GetResource(ResourceType.Gold);

        bool canUpgrade = _upgrade.CanUpgrade(currentLevel, currentGold);
        _upgradeButton.interactable = canUpgrade;
    }

    public void OnClickUpgrade()
    {
        UpgradeManager.Instance.TryUpgrade(_upgradeType);
    }

    public void Lock()
    {
        SetLock(true);
    }

    public void Unlock()
    {
        SetLock(false);
    }

    public void SetLock(bool isLocked)
    {
        if (_lockImage == null) return;

        _lockImage.SetActive(isLocked);
        _upgradeButton.interactable = !isLocked && CanCurrentlyUpgrade();
    }

    public bool IsLocked()
    {
        return _lockImage != null && _lockImage.activeSelf;
    }

    private bool CanCurrentlyUpgrade()
    {
        if (_upgrade == null) return false;

        int currentLevel = UpgradeManager.Instance.GetCurrentLevel(_upgradeType);
        float currentGold = (float)ResourceManager.Instance.GetResource(ResourceType.Gold);

        return _upgrade.CanUpgrade(currentLevel, currentGold);
    }
}
