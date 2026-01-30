using UnityEngine;
using System;
using System.Collections.Generic;

public class ResourceData : IUserData
{
    public Dictionary<ResourceType, float> Resources { get; private set; } = new();

    public void SetDefaultData()
    {
        Resources.Clear();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = 0f;
        }
        
        // NOTE : 잠시 테스트 용도로 골드 세팅
        Resources[ResourceType.Gold] = 10000f;
    }

    public bool LoadData()
    {
        Resources.Clear();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = PlayerPrefs.GetFloat(type.ToString(), 0f);
        }
        return true;
    }

    public bool SaveData()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = (float)ResourceManager.Instance.GetResource(type);
        }

        foreach (var pair in Resources)
        {
            PlayerPrefs.SetFloat(pair.Key.ToString(), pair.Value);
        }
        PlayerPrefs.Save();
        return true;
    }
}
