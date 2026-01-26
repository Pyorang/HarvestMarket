using UnityEngine;

public class Farm : MonoBehaviour
{
    [SerializeField] private Animal _animalType;
    [SerializeField] private GameObject[] _animals;

    public string AnimalType => _animalType.ToString();
    public GameObject[] Animals => _animals;

    private void Start()
    {
        InitializeAnimals();
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
