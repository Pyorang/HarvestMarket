using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScalePop))]
public class CurrencyUI : MonoBehaviour
{
    [Header("재화량 표시 UI")]
    [Space]
    [SerializeField] private ResourceType _resourceType = ResourceType.Egg;
    [SerializeField] private TextMeshProUGUI _text;

    private ScalePop _scalePop;

    private void Awake()
    {
        _scalePop = GetComponent<ScalePop>();
        ResourceManager.OnResourceChanged += UpdateResourceText;
    }

    public void UpdateResourceText(ResourceType type, int amount)
    {
        if (type == _resourceType)
        {
            _text.text = amount.ToString();
            _scalePop.Pop();
        }
    }

    private void OnDisable()
    {
        ResourceManager.OnResourceChanged -= UpdateResourceText;
    }
}

