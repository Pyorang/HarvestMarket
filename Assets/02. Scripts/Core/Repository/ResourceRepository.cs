using System;
using UnityEngine;

public class ResourceRepository : IRepository<ResourceData>
{
    private const string KEY_PREFIX = "Resource_";
    private const string EXISTS_KEY = "Resource_Initialized";

    public ResourceData Load()
    {
        var data = new ResourceData();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            data.Resources[type] = PlayerPrefs.GetFloat($"{KEY_PREFIX}{type}", 0f);
        }
        return data;
    }

    public void Save(ResourceData data)
    {
        foreach (var pair in data.Resources)
        {
            PlayerPrefs.SetFloat($"{KEY_PREFIX}{pair.Key}", pair.Value);
        }
        PlayerPrefs.SetInt(EXISTS_KEY, 1);
        PlayerPrefs.Save();
    }

    public void Delete()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            PlayerPrefs.DeleteKey($"{KEY_PREFIX}{type}");
        }
        PlayerPrefs.DeleteKey(EXISTS_KEY);
        PlayerPrefs.Save();
    }

    public bool Exists() => PlayerPrefs.HasKey(EXISTS_KEY);
}
