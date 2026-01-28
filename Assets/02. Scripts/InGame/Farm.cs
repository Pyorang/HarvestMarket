using UnityEngine;

public class Farm : MonoBehaviour
{
    private double _clickReward = 999;
    private double _autoReward = 1000;
    private float _timeElapsed = 0f;
    private static readonly float _autoInterval = 1.0f;

    public double ClickReward => _clickReward;

    [Header("보상 종류")]
    [Space]
    [SerializeField] private ResourceType _resourceType;

    public ResourceType Resource => _resourceType;

    [Header ("동물 오브젝트들")]
    [Space]
    [SerializeField] private GameObject[] _animals;

    public GameObject[] Animals => _animals;

    private TextFeedback _textFeedback;

    private void Awake()
    {
        _textFeedback = GetComponent<TextFeedback>();
    }

    private void Start()
    {
        //InitializeAnimals();
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;

        if(_timeElapsed >= _autoInterval)
        {
            double reward = _autoReward * FeverManager.Instance.BonusMultiplier;

            ResourceManager.Instance.AddResource(_resourceType, reward);

            if (_textFeedback != null)
            {
                ClickInfo clickInfo = new ClickInfo(_resourceType, reward);
                _textFeedback.Play(clickInfo);
            }

            _timeElapsed = 0f;
        }
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
