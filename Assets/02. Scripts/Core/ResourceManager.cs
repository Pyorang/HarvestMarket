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

    private Dictionary<ResourceType, int> _resources = new();

    public event Action<ResourceType, int> OnResourceChanged;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeResources();
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeResources()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            _resources[type] = 0;
            OnResourceChanged?.Invoke(type, _resources[type]);
        }
    }

    public int GetResource(ResourceType type) => _resources[type];

    public void AddResource(ResourceType type, int amount)
    {
        _resources[type] = Mathf.Max(0, _resources[type] + amount);
        OnResourceChanged?.Invoke(type, _resources[type]);
    }

    public bool TrySpendResource(ResourceType type, int amount)
    {
        if (_resources[type] >= amount)
        {
            _resources[type] -= amount;
            OnResourceChanged?.Invoke(type, _resources[type]);
            return true;
        }
        return false;
    }

    public void SetResource(ResourceType type, int amount)
    {
        _resources[type] = Mathf.Max(0, amount);
        OnResourceChanged?.Invoke(type, _resources[type]);
    }
}
