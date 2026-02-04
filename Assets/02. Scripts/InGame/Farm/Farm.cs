using UnityEngine;

public class Farm : MonoBehaviour
{
    [Header("보상 종류")]
    [SerializeField] private CurrencyType _currencyType;

    [Header("동물 오브젝트들")]
    [SerializeField] private GameObject[] _animals;

    public CurrencyType Currency => _currencyType;
    public GameObject[] Animals => _animals;

    private TextFeedback _textFeedback;
    private float _timeElapsed = 0f;
    private static readonly float _autoInterval = 1.0f;

    public double ClickReward
    {
        get
        {
            var upgrade = UpgradeManager.Instance.GetUpgrade(GetClickUpgradeType());
            return upgrade?.GetTotalValue() ?? 0;
        }
    }

    private double AutoReward
    {
        get
        {
            var upgrade = UpgradeManager.Instance.GetUpgrade(GetAutoUpgradeType());
            return upgrade?.GetTotalValue() ?? 0;
        }
    }

    private void Awake()
    {
        _textFeedback = GetComponent<TextFeedback>();
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed >= _autoInterval)
        {
            double reward = AutoReward * FeverManager.Instance.BonusMultiplier;

            if (reward > 0)
            {
                CurrencyManager.Instance.AddCurrency(_currencyType, reward);

                if (_textFeedback != null)
                {
                    ClickInfo clickInfo = new ClickInfo(_currencyType, reward);
                    _textFeedback.Play(clickInfo);
                }
            }

            _timeElapsed = 0f;
        }
    }

    private UpgradeType GetClickUpgradeType()
    {
        return _currencyType switch
        {
            CurrencyType.Egg => UpgradeType.EggBasket,
            CurrencyType.Meat => UpgradeType.ButcherKnife,
            CurrencyType.Milk => UpgradeType.MilkBucket,
            _ => UpgradeType.EggBasket
        };
    }

    private UpgradeType GetAutoUpgradeType()
    {
        return _currencyType switch
        {
            CurrencyType.Egg => UpgradeType.ChickenCoop,
            CurrencyType.Meat => UpgradeType.Pigpen,
            CurrencyType.Milk => UpgradeType.CattleBarn,
            _ => UpgradeType.ChickenCoop
        };
    }

    private void InitializeAnimals()
    {
        for (int i = 0; i < _animals.Length; i++)
        {
            _animals[i].SetActive(i == 0);
        }
    }

    public bool TryActivateNextAnimal()
    {
        foreach (var animal in _animals)
        {
            if (!animal.activeSelf)
            {
                animal.SetActive(true);
                return true;
            }
        }

        return false;
    }
}
