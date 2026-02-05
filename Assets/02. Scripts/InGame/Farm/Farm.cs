using UnityEngine;

public class Farm : MonoBehaviour
{
    [Header("보상 종류")]
    [SerializeField] private CurrencyType _currencyType;

    [Header("동물 오브젝트들")]
    [SerializeField] private GameObject[] _animals;

    public CurrencyType Currency => _currencyType;
    public GameObject[] Animals => _animals;

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
