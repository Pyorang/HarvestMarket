using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScalePop))]
public class CurrencyUI : MonoBehaviour
{
    [Header("재화량 표시 UI")]
    [Space]
    [SerializeField] private CurrencyType _currencyType = CurrencyType.Egg;
    [SerializeField] private TextMeshProUGUI _text;

    private ScalePop _scalePop;

    private void Awake()
    {
        _scalePop = GetComponent<ScalePop>();
        CurrencyManager.OnCurrencyChanged += UpdateCurrencyText;
    }

    public void UpdateCurrencyText(CurrencyType type, double amount)
    {
        if (type == _currencyType)
        {
            _text.text = amount.ToFormattedString();
            _scalePop.Pop();
        }
    }

    private void OnDisable()
    {
        CurrencyManager.OnCurrencyChanged -= UpdateCurrencyText;
    }
}

