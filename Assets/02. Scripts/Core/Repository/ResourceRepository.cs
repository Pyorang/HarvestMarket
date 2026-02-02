using System;
using UnityEngine;

public class ResourceRepository : IRepository<ResourceData>
{
    private readonly string _keyPrefix;
    private readonly string _existsKey;

    public ResourceRepository(string userKey = "")
    {
        var prefix = string.IsNullOrEmpty(userKey) ? "" : userKey + "_";
        _keyPrefix = prefix + "Resource_";
        _existsKey = prefix + "Resource_Initialized";
    }

    public ResourceData Load()
    {
        var data = new ResourceData();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            data.Resources[type] = PlayerPrefs.GetFloat($"{_keyPrefix}{type}", 0f);
        }
        return data;
    }

    public void Save(ResourceData data)
    {
        foreach (var pair in data.Resources)
        {
            PlayerPrefs.SetFloat($"{_keyPrefix}{pair.Key}", pair.Value);
        }
        PlayerPrefs.SetInt(_existsKey, 1);
        PlayerPrefs.Save();
    }

    public void Delete()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            PlayerPrefs.DeleteKey($"{_keyPrefix}{type}");
        }
        PlayerPrefs.DeleteKey(_existsKey);
        PlayerPrefs.Save();
    }

    public bool Exists() => PlayerPrefs.HasKey(_existsKey);
}
