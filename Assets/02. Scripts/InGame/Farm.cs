using UnityEngine;

public class Farm : MonoBehaviour
{
    [Header("보상 종류")]
    [SerializeField] private ResourceType _resourceType;

    [Header("동물 오브젝트들")]
    [SerializeField] private GameObject[] _animals;

    public ResourceType Resource => _resourceType;
    public GameObject[] Animals => _animals;

    private TextFeedback _textFeedback;
    private float _timeElapsed = 0f;
    private static readonly float _autoInterval = 1.0f;

    public double ClickReward
    {
        get
        {
            UpgradeType clickUpgrade = GetClickUpgradeType();
            int level = UpgradeManager.Instance.GetCurrentLevel(clickUpgrade);
            var upgrade = UpgradeManager.Instance.GetUpgrade(clickUpgrade);
            return upgrade.GetTotalValue(level);
        }
    }

    private double AutoReward
    {
        get
        {
            UpgradeType autoUpgrade = GetAutoUpgradeType();
            int level = UpgradeManager.Instance.GetCurrentLevel(autoUpgrade);
            var upgrade = UpgradeManager.Instance.GetUpgrade(autoUpgrade);
            return upgrade.GetTotalValue(level);
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
                ResourceManager.Instance.AddResource(_resourceType, reward);

                if (_textFeedback != null)
                {
                    ClickInfo clickInfo = new ClickInfo(_resourceType, reward);
                    _textFeedback.Play(clickInfo);
                }
            }

            _timeElapsed = 0f;
        }
    }

    private UpgradeType GetClickUpgradeType()
    {
        return _resourceType switch
        {
            ResourceType.Egg => UpgradeType.EggBasket,
            ResourceType.Meat => UpgradeType.ButcherKnife,
            ResourceType.Milk => UpgradeType.MilkBucket,
            _ => UpgradeType.EggBasket
        };
    }

    private UpgradeType GetAutoUpgradeType()
    {
        return _resourceType switch
        {
            ResourceType.Egg => UpgradeType.ChickenCoop,
            ResourceType.Meat => UpgradeType.Pigpen,
            ResourceType.Milk => UpgradeType.CattleBarn,
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
