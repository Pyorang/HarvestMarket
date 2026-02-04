using System;
using System.Collections.Generic;

public class CurrencyData
{
    public Dictionary<CurrencyType, float> Currencies { get; set; } = new();

    public void SetDefault()
    {
        Currencies.Clear();
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            Currencies[type] = 0f;
        }
        
        // NOTE: Test purpose
        Currencies[CurrencyType.Gold] = 10000f;
    }
}
