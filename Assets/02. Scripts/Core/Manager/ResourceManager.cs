using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager s_instance;
    public static ResourceManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("ResourceManager");
                s_instance = go.AddComponent<ResourceManager>();
                DontDestroyOnLoad(go);
            }
            return s_instance;
        }
    }

    private Dictionary<ResourceType, double> _resources = new();

    public static event Action<ResourceType, double> OnResourceChanged;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeFromUserData();
    }

    private void InitializeFromUserData()
    {
        var resourceData = UserDataManager.Instance.GetUserData<ResourceData>();

        if (resourceData != null)
        {
            foreach (var pair in resourceData.Resources)
            {
                _resources[pair.Key] = pair.Value;
                OnResourceChanged?.Invoke(pair.Key, pair.Value);
            }
        }
        else
        {
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                _resources[type] = 0;
                OnResourceChanged?.Invoke(type, 0);
            }
        }
    }

    public double GetResource(ResourceType type) => _resources[type];

    public void AddResource(ResourceType type, double amount)
    {
        if(amount <= 0) return;
        _resources[type] = System.Math.Max(0, _resources[type] + amount);
        OnResourceChanged?.Invoke(type, _resources[type]);
    }

    public bool TrySpendResource(ResourceType type, double amount)
    {
        if (_resources[type] >= amount)
        {
            _resources[type] -= amount;
            OnResourceChanged?.Invoke(type, _resources[type]);
            return true;
        }
        return false;
    }

    public void SetResource(ResourceType type, double amount)
    {
        _resources[type] = System.Math.Max(0, amount);
        OnResourceChanged?.Invoke(type, _resources[type]);
    }
}
