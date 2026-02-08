using System;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CurrencyData
{
    [FirestoreProperty]
    public Dictionary<string, float> Currencies { get; set; } = new();

    [FirestoreProperty]
    public long LastSavedAt { get; set; } = 0;

    public CurrencyData() { }

    public void SetDefault()
    {
        Currencies.Clear();
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            Currencies[type.ToString()] = 0f;
        }

        // NOTE: Test purpose
        Currencies[CurrencyType.Gold.ToString()] = 10000f;
    }

    public float GetAmount(CurrencyType type)
    {
        string key = type.ToString();
        return Currencies.TryGetValue(key, out float amount) ? amount : 0f;
    }

    public void SetAmount(CurrencyType type, float amount)
    {
        Currencies[type.ToString()] = amount;
    }

    public void AddAmount(CurrencyType type, float amount)
    {
        string key = type.ToString();
        if (Currencies.ContainsKey(key))
            Currencies[key] += amount;
        else
            Currencies[key] = amount;
    }

    public bool SpendCurrency(CurrencyType type, float amount)
    {
        string key = type.ToString();
        if (Currencies.TryGetValue(key, out float currentAmount) && currentAmount >= amount)
        {
            Currencies[key] = currentAmount - amount;
            return true;
        }
        return false;
    }
}