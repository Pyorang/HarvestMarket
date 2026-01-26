using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [Header("재화량 표시 UI")]
    [Space]
    [SerializeField] private TextMeshProUGUI _eggText;

    private void Start()
    {
        ResourceManager.Instance.OnResourceChanged += UpdateResourceText;
    }

    public void UpdateResourceText(ResourceType type, int amount)
    {
        if (type == ResourceType.Egg)
        {
            _eggText.text = $"{amount.ToString()}";
        }
    }

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceChanged -= UpdateResourceText;
    }
}
