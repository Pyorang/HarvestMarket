using UnityEngine;

public class Farm : MonoBehaviour
{
    // NOTE: 농장에서 키우는 동물의 종류를 선택하고, 늘리거나 줄일 수 있게 하는 역할.

    [SerializeField] private Animal _animalType;

    [SerializeField] private GameObject[] _animals;


}
