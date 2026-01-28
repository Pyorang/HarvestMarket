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
        // ResourceManager에서 현재 값 동기화
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
