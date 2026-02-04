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

    private void Start()
    {
        if (UpgradeManager.Instance.IsInitialized)
        {
            InitializeUI();
        }
        else
        {
            UpgradeManager.OnInitialized += InitializeUI;
        }
    }

    private void OnEnable()
    {
        CurrencyManager.OnCurrencyChanged += OnCurrencyChanged;
        UpgradeManager.OnUpgraded += OnUpgraded;

        if (UpgradeManager.Instance != null && UpgradeManager.Instance.IsInitialized)
        {
            RefreshUI();
        }
    }

    private void OnDisable()
    {
        CurrencyManager.OnCurrencyChanged -= OnCurrencyChanged;
        UpgradeManager.OnUpgraded -= OnUpgraded;
    }

    private void OnDestroy()
    {
        UpgradeManager.OnInitialized -= InitializeUI;
    }

    private void InitializeUI()
    {
        UpgradeManager.OnInitialized -= InitializeUI;
        RefreshUI();
    }

    private void OnCurrencyChanged(CurrencyType type, double amount)
    {
        if (type == CurrencyType.Gold)
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

        _titleText.text = $"{_upgrade.Name} (Lv. {_upgrade.CurrentLevel})";

        if (_upgrade.IsMaxLevel())
        {
            _costText.text = "MAX";
        }
        else
        {
            float cost = _upgrade.GetCost();
            _costText.text = $"{((double)cost).ToFormattedString()}";
        }

        RefreshButtonState();
    }

    private void RefreshButtonState()
    {
        if (_upgrade == null || _upgradeButton == null) return;

        float currentGold = (float)CurrencyManager.Instance.GetCurrency(CurrencyType.Gold);
        _upgradeButton.interactable = _upgrade.CanUpgrade(currentGold);
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

        float currentGold = (float)CurrencyManager.Instance.GetCurrency(CurrencyType.Gold);
        return _upgrade.CanUpgrade(currentGold);
    }
}
