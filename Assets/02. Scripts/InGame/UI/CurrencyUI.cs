using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [Header("재화량 표시 UI")]
    [Space]
    [SerializeField] private ResourceType _resourceType = ResourceType.Egg;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        ResourceManager.Instance.OnResourceChanged += UpdateResourceText;
    }

    public void UpdateResourceText(ResourceType type, int amount)
    {
        if (type == _resourceType)
        {
            _text.text = $"{amount.ToString()}";
        }
    }

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceChanged -= UpdateResourceText;
    }
}
