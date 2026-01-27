using UnityEngine;

public class Farm : MonoBehaviour
{
    private int _clickReward = 1;
    private int _autoReward = 0;
    private float _timeElapsed = 0f;
    private static readonly float _autoInterval = 1.0f;

    public int ClickReward => _clickReward;

    [Header("보상 종류")]
    [Space]
    [SerializeField] private ResourceType _resourceType;

    public ResourceType Resource => _resourceType;

    [Header ("동물 오브젝트들")]
    [Space]
    [SerializeField] private GameObject[] _animals;

    public GameObject[] Animals => _animals;

    private void Start()
    {
        //InitializeAnimals();
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;

        if(_timeElapsed >= _autoInterval)
        {
            ResourceManager.Instance.AddResource(_resourceType, _autoReward);
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
