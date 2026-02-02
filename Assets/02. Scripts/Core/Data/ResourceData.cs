using System;
using System.Collections.Generic;

public class ResourceData
{
    public Dictionary<ResourceType, float> Resources { get; set; } = new();

    public void SetDefault()
    {
        Resources.Clear();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = 0f;
        }
        
        // NOTE: Test purpose
        Resources[ResourceType.Gold] = 10000f;
    }
}
